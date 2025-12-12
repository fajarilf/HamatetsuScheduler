using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Part> Parts { get; set; } = new List<Part>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
