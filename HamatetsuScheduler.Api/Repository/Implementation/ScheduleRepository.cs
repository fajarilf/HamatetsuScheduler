using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class ScheduleRepository: IRepository<Schedule>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Schedule> _dbContextSet;

        public ScheduleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = _dbContext.Set<Schedule>();
        }

        public DbSet<Schedule> Dbset => _dbContextSet;

        public async Task Delete(Schedule entity)
        {
            _dbContext.Schedules.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Schedule> SaveAsync(Schedule entity)
        {
            await _dbContext.Schedules.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Schedule>> SaveRangeAsync(IEnumerable<Schedule> entities)
        {
            await _dbContext.Schedules.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
