using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAMiniEHR.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly MINIDbContext _context;

        public CreateModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = new();

        public SelectList PatientList { get; set; } = null!;

        public void OnGet()
        {
            LoadPatients();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 🔴 IMPORTANT: reload dropdown
            LoadPatients();

            if (!ModelState.IsValid)
                return Page();

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadPatients()
        {
            PatientList = new SelectList(
                _context.Patients.ToList(),
                "PatientID",
                "FirstName");
        }
    }
}
