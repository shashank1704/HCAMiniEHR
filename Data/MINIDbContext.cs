using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Data
{
    public class MINIDbContext : DbContext
    {
        public MINIDbContext(DbContextOptions<MINIDbContext> options)
            : base(options)
        {
        }

        // DbSets (Tables)
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }   
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Default schema = Mini
            modelBuilder.HasDefaultSchema("Mini");

            // Patient → Appointment (1-to-many)
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment → LabOrder (1-to-many)
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.LabOrders)
                .WithOne(l => l.Appointment)
                .HasForeignKey(l => l.AppointmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Patients)
                 .WithOne(p => p.Doctor)
                  .HasForeignKey(p => p.DoctorID);


            // VERY IMPORTANT: Tell EF Core this table has a trigger
            modelBuilder.Entity<Appointment>()
                .ToTable(tb => tb.HasTrigger("trg_Appointment_Audit"));
        }
    }
}
