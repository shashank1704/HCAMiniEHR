using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class IndexModel : PageModel
    {
        private readonly MINIDbContext _context;

        public IndexModel(MINIDbContext context)
        {
            _context = context;
        }

        public IList<LabOrder> LabOrders { get; set; } = new List<LabOrder>();

        public async Task OnGetAsync()
        {
            LabOrders = await _context.LabOrders
                .Include(l => l.Appointment)
                .OrderByDescending(l => l.OrderedAt)
                .ToListAsync();
        }
    }
}
