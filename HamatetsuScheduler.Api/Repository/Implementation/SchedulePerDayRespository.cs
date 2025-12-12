using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class SchedulePerDayRespository : IRepository<SchedulePerDay>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<SchedulePerDay> _dbSet;

        public SchedulePerDayRespository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<SchedulePerDay>();
        }

        public DbSet<SchedulePerDay> Dbset => _dbSet;

        public async Task Delete(SchedulePerDay entity)
        {
            _dbContext.SchedulePerDays.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRange(IEnumerable<SchedulePerDay> entities)
        {
            _dbContext.SchedulePerDays.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SchedulePerDay> SaveAsync(SchedulePerDay entity)
        {
            await _dbContext.SchedulePerDays.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<SchedulePerDay>> SaveRangeAsync(IEnumerable<SchedulePerDay> entities)
        {
            await _dbContext.SchedulePerDays.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
