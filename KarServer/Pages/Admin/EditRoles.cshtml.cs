using KarServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KarServer.Data;
using KarServer.Models;


namespace YourApp.Pages.Admin
{
    [Authorize(Roles = "User Manager, Application Manager")]
    public class EditRolesModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditRolesModel(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [BindProperty]
        public List<RoleViewModel> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserId = user.Id;
            UserName = user.UserName;
            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            Roles = allRoles.Select(r => new RoleViewModel
            {
                Name = r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            var selectedRoles = Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(selectedRoles).ToList();

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            return RedirectToPage("Users");
        }
    }

    public class RoleViewModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}