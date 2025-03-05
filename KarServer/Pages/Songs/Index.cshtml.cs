// Pages/Songs/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KarServer.Data;
using KarServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarServer.Pages.Songs
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Song> Songs { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var songs = from s in _context.Songs select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                songs = songs.Where(s => s.Title.Contains(SearchString) ||
                                        s.Artist.Contains(SearchString) ||
                                        s.Genre.Contains(SearchString));
            }
            Songs = await songs.ToListAsync();
        }
    }
}