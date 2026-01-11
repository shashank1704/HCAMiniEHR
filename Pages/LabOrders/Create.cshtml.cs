using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly MINIDbContext _context;

        public CreateModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LabOrder LabOrder { get; set; } = new();

        public SelectList AppointmentList { get; set; } = null!;

        public void OnGet()
        {
            LoadAppointments();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadAppointments();

            if (!ModelState.IsValid)
                return Page();

            _context.LabOrders.Add(LabOrder);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadAppointments()
        {
            AppointmentList = new SelectList(
                _context.Appointments,
                "AppointmentID",
                "AppointmentID");
        }
    }
}

