using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KarServer.Data;
using KarServer.Models;


namespace KarServer.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public string WelcomeMessage { get; set; }

        public void OnGet()
        {
            WelcomeMessage = $"Welcome, {User.Identity.Name}!";
        }
    }
}