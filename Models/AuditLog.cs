using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCAMiniEHR.Models
{
    [Table("AuditLog", Schema = "Mini")]
    public class AuditLog
    {
        [Key]
        public int AuditID { get; set; }

        public string? TableName { get; set; }
        public string? ActionType { get; set; }   // INSERT / UPDATE / DELETE
        public int? RecordID { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        public DateTime ChangedAt { get; set; }
    }
}
