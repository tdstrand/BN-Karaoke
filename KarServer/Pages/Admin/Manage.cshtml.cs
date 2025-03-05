// Pages/Admin/Manage.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KarServer.Data;
using KarServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarServer.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        private readonly AppDbContext _context;

        public ManageModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<SongQueue> Queue { get; set; }
        public IList<Song> Songs { get; set; }

        public async Task OnGetAsync()
        {
            Queue = await _context.SongQueue.Include(q => q.Song).ToListAsync();
            Songs = await _context.Songs.ToListAsync();
        }

        public async Task<IActionResult> OnPostClearQueueAsync()
        {
            _context.SongQueue.RemoveRange(_context.SongQueue);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteSongAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}