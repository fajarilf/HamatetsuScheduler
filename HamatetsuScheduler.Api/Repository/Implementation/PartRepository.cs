using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class PartRepository : IRepository<Part>
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Part> _dbContextSet;

        public PartRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContextSet = _dbContext.Set<Part>();
        }

        public DbSet<Part> Dbset => _dbContextSet;
        public AppDbContext DbContext => _dbContext;

        public async Task Delete(Part entity)
        {
            _dbContext.Parts.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Part> SaveAsync(Part entity)
        {
            await _dbContext.Parts.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(entity)
                .Reference(d => d.Customer)
                .LoadAsync();

            return entity;
        }

        public async Task<IEnumerable<Part>> SaveRangeAsync(IEnumerable<Part> entities)
        {
            await _dbContext.Parts.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }
    }
}
