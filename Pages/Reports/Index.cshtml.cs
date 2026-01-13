using HCAMiniEHR.Data;
using HCAMiniEHR.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly MINIDbContext _context;

        public IndexModel(MINIDbContext context)
        {
            _context = context;
        }

        // Report 1: Pending Lab Orders
        public IList<PendingLabOrderDTO> PendingLabOrders { get; set; } = new List<PendingLabOrderDTO>();

        // Report 2: Patients without future appointments
        public IList<PatientNoFollowUpDTO> PatientsWithoutFollowUp { get; set; } = new List<PatientNoFollowUpDTO>();

        // Report 3: Appointments per day
        public IList<AppointmentCountDTO> AppointmentsPerDay { get; set; } = new List<AppointmentCountDTO>();

        public async Task OnGetAsync()
        {
            // 🔹 Report 1: Pending Lab Orders
            PendingLabOrders = await _context.LabOrders
                .Where(l => l.Status == "Pending")
                .Select(l => new PendingLabOrderDTO
                {
                    TestName = l.TestName,
                    AppointmentID = l.AppointmentID
                })
                .ToListAsync();

            // 🔹 Report 2: Patients without future appointments
            PatientsWithoutFollowUp = await _context.Patients
                .Where(p => !_context.Appointments
                    .Any(a => a.PatientID == p.PatientID &&
                              a.AppointmentDate > DateTime.Now))
                .Select(p => new PatientNoFollowUpDTO
                {
                    PatientName = p.FirstName + " " + p.LastName
                })
                .ToListAsync();

            // 🔹 Report 3: Appointments grouped by date
            AppointmentsPerDay = await _context.Appointments
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new AppointmentCountDTO
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }
    }
}

