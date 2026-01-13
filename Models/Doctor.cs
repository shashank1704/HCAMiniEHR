using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("Doctor", Schema = "Mini")]
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = "";
        public string? Specialization { get; set; }

        public ICollection<Patient>? Patients { get; set; }
    }
}

