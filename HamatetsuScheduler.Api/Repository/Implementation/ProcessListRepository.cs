using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class ProcessListRepository: IRepository<ProcessList>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<ProcessList> _dbContextSet;

        public ProcessListRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = _dbContext.Set<ProcessList>();
        }

        public DbSet<ProcessList> Dbset => _dbContextSet;
        public AppDbContext DbContext => _dbContext;

        public async Task Delete(ProcessList entity)
        {
            _dbContext.ProcessLists.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRange(List<ProcessList> entities)
        {
            _dbContext.ProcessLists.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProcessList> SaveAsync(ProcessList entity)
        {
            await _dbContext.ProcessLists.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(entity)
                 .Reference(e => e.Customer)
                 .LoadAsync();

            await _dbContext.Entry(entity)
                .Reference(e => e.Process)
                .LoadAsync();

            await _dbContext.Entry(entity)
                .Reference(e => e.Part)
                .LoadAsync();

            return entity;
        }

        public async Task<IEnumerable<ProcessList>> SaveRangeAsync(IEnumerable<ProcessList> entities)
        {
            await _dbContext.ProcessLists.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
