using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("Appointment", Schema = "Mini")]
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string? DoctorName { get; set; }
        public string Status { get; set; } = "Scheduled";

        public Patient? Patient { get; set; }
        public ICollection<LabOrder>? LabOrders { get; set; }
    }
}

