using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("schedule_per_days")]
    public class SchedulePerDay
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        // navigation
        public Schedule Schedule { get; set; } = null!;
        public ICollection<ScheduleDetail> Details { get; set; } = [];
    }
}
