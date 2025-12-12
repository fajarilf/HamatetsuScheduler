using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("parts")]
    public class Part
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
        public ICollection<ProcessList> ProcessLists { get; set; } = [];
        public ICollection<Schedule> Schedules { get; set; } = [];
    }
}
