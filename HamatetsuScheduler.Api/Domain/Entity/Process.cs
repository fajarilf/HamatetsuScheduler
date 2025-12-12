using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("process")]
    public class Process
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<ProcessList> ProcessLists { get; set; } = new List<ProcessList>();
        public ICollection<ScheduleDetail> ScheduleDetails { get; set; } = new List<ScheduleDetail>();
    }
}
