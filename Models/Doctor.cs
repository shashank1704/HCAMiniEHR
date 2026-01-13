using System.ComponentModel.DataAnnotations;
using HCAMiniEHR.Models;

public class Doctor
{
    [Key]
    public int DoctorID { get; set; }
    public string DoctorName { get; set; } = "";
    public string? Specialization { get; set; }

    public ICollection<Patient>? Patients { get; set; }
}

