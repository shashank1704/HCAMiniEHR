using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("Patient", Schema = "Mini")]
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Only letters, spaces, hyphens, and apostrophes are allowed.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Only letters, spaces, hyphens, and apostrophes are allowed.")]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }


        public int? DoctorID { get; set; }
        public Doctor? Doctor { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}

