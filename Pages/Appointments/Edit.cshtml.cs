using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly MINIDbContext _context;

        public EditModel(MINIDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public SelectList PatientList { get; set; } = null!;
        public SelectList DoctorList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Appointment = await _context.Appointments
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (Appointment == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return Page();
            }

            // Check for duplicates
            bool isPatientBooked = await _context.Appointments.AnyAsync(a => 
                a.AppointmentID != Appointment.AppointmentID &&
                a.PatientID == Appointment.PatientID && 
                a.AppointmentDate == Appointment.AppointmentDate);

            bool isDoctorBooked = await _context.Appointments.AnyAsync(a => 
                a.AppointmentID != Appointment.AppointmentID &&
                a.DoctorName == Appointment.DoctorName && 
                a.AppointmentDate == Appointment.AppointmentDate);

            if (isPatientBooked)
            {
                ModelState.AddModelError("Appointment.PatientID", "Patient already has an appointment at this time.");
            }

            if (isDoctorBooked)
            {
                ModelState.AddModelError("Appointment.DoctorName", "Doctor is already booked at this time.");
            }

            if (isPatientBooked || isDoctorBooked)
            {
                LoadDropdowns();
                return Page();
            }

            _context.Attach(Appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(Appointment.AppointmentID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        private void LoadDropdowns()
        {
            PatientList = new SelectList(_context.Patients.ToList(), "PatientID", "FirstName");
            DoctorList = new SelectList(_context.Doctors.ToList(), "DoctorName", "DoctorName");
        }
    }
}
