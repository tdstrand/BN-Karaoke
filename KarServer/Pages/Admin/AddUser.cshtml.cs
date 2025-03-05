using KarServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using KarServer.Data;
using KarServer.Data;

namespace YourApp.Pages.Admin
{
    [Authorize(Roles = "User Manager, Application Manager")]
    public class AddUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public AddUserViewModel Input { get; set; }

        public List<string> Roles { get; set; }

        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            Input = new AddUserViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.UserName, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.SelectedRoles != null && Input.SelectedRoles.Count > 0)
                    {
                        await _userManager.AddToRolesAsync(user, Input.SelectedRoles);
                    }
                    return RedirectToPage("Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Page();
        }
    }

    public class AddUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}