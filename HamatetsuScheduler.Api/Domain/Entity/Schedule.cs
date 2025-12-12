using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("schedules")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("part_id")]
        public int PartId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("month")]
        public int Month {  get; set; }

        [Column("year")]
        public int Year { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Part Part { get; set; } = null!;
        public Customer Customer { get; set; } = null!;
        public ICollection<SchedulePerDay> Schedules { get; set; } = new List<SchedulePerDay>();
    }
}
