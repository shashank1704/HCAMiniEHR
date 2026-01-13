using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Patients
{
    public class DeleteModel : PageModel
    {
        private readonly MINIDbContext _context;

        public DeleteModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patient = await _context.Patients.FindAsync(id);
            if (Patient == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientID == id);
            
            if (hasAppointments)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete patient because they have existing appointments.");
                
                // Re-fetch patient for the UI
                Patient = await _context.Patients.FindAsync(id);
                return Page();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}

