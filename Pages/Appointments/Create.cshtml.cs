using HCAMiniEHR.Data;
using HCAMiniEHR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public SelectList DoctorList { get; set; } = null!;

        public void OnGet()
        {
            Appointment.AppointmentDate = DateTime.Now;
            LoadDropdowns();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //LoadPatients();

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return Page();
            }

            // Check for duplicates
            bool isPatientBooked = await _context.Appointments.AnyAsync(a => 
                a.PatientID == Appointment.PatientID && 
                a.AppointmentDate == Appointment.AppointmentDate);

            bool isDoctorBooked = await _context.Appointments.AnyAsync(a => 
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

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private void LoadDropdowns()
        {
            PatientList = new SelectList(
                _context.Patients.ToList(),
                "PatientID",
                "FirstName");

            DoctorList = new SelectList(
                _context.Doctors.ToList(),
                "DoctorName",
                "DoctorName");
        }
    }
}
