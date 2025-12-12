using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamatetsuScheduler.Api.Domain.Entity
{
    [Table("process_lists")]
    public class ProcessList
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("process_id")]
        public int ProcessId { get; set; }

        [Column("part_id")]
        public int PartId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
        public Process Process { get; set; } = null!;
        public Part Part { get; set; } = null!;
    }
}