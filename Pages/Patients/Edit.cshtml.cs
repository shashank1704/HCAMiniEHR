using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCAMiniEHR.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly MINIDbContext _context;

        public EditModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public SelectList DoctorList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patient = await _context.Patients.FindAsync(id);
            if (Patient == null) return NotFound();
            
            LoadDoctors();
            return Page();
        }

        private void LoadDoctors()
        {
            DoctorList = new SelectList(_context.Doctors.ToList(), "DoctorID", "DoctorName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
            {
                LoadDoctors();
                return Page();
            }

            _context.Update(Patient);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}

