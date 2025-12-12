using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("schedule_details")]
    public class ScheduleDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("schedule_per_day_id")]
        public int SchedulePerDayId { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("process_id")]
        public int ProcessId { get; set; }

        [Column("target_quantity_total")]
        public int TargetQuantityTotal { get; set; }

        [Column("target_quantity_per_day")]
        public int TargetQuantityPerDay { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("finish_time")]
        public DateTime FinishTime { get; set; }

        // Navigation
        public SchedulePerDay SchedulePerDay { get; set; } = null!;
        public Process Process { get; set; } = null!;
    }
}