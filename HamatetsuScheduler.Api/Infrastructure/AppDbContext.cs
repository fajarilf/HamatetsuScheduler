using HamatetsuScheduler.Api.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Infrastructure
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts): base(opts)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessList> ProcessLists { get; set; }
        public DbSet<SchedulePerDay> SchedulePerDays { get; set; }
        public DbSet<ScheduleDetail> ScheduleDetails { get; set; }
    }
}
