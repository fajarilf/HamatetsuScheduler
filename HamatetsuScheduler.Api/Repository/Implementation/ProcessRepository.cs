using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class ProcessRepository : IRepository<Process>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Process> _dbContextSet;

        public ProcessRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = _dbContext.Set<Process>();
        }

        public DbSet<Process> Dbset => _dbContextSet;
        public DbContext DbContext => _dbContext;
        public async Task Delete(Process entity)
        {
            _dbContext.Processes.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Process> SaveAsync(Process entity)
        {
            await _dbContext.Processes.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Process>> SaveRangeAsync(IEnumerable<Process> entities)
        {
            await _dbContext.Processes.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
