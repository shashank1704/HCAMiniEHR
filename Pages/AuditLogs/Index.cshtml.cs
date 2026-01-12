using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.AuditLogs
{
    public class IndexModel : PageModel
    {
        private readonly MINIDbContext _context;

        public IndexModel(MINIDbContext context)
        {
            _context = context;
        }

        public IList<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

        public async Task OnGetAsync()
        {
            AuditLogs = await _context.AuditLogs
                .OrderByDescending(a => a.ChangedAt)
                .ToListAsync();
        }
    }
}

