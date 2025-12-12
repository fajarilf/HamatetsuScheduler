using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class ScheduleDetailRepository: IRepository<ScheduleDetail>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<ScheduleDetail> _dbContextSet;

        public ScheduleDetailRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = _dbContext.Set<ScheduleDetail>();
        }

        public DbSet<ScheduleDetail> Dbset => _dbContextSet;

        public async Task Delete(ScheduleDetail entity)
        {
            _dbContext.ScheduleDetails.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRange(List<ScheduleDetail> entities)
        {
            _dbContext.ScheduleDetails.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ScheduleDetail> SaveAsync(ScheduleDetail entity)
        {
            await _dbContext.ScheduleDetails.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ScheduleDetail>> SaveRangeAsync(IEnumerable<ScheduleDetail> entities)
        {
            await _dbContext.ScheduleDetails.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
