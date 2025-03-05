// Pages/Queue/Index.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KarServer.Data;
using KarServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KarServer.Pages.Queue
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<SongQueue> Queue { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Queue = await _context.SongQueue
                .Where(q => q.UserId == userId)
                .Include(q => q.Song)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var queueItem = await _context.SongQueue.FindAsync(id);
            if (queueItem != null && queueItem.UserId == userId)
            {
                _context.SongQueue.Remove(queueItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}