using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("LabOrder", Schema = "Mini")]
    public class LabOrder
    {
        [Key]
        public int LabOrderID { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentID { get; set; }

        public string TestName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string? ResultValue { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.Now;

        public Appointment? Appointment { get; set; }
    }
}

