using HamatetsuScheduler.Api.Domain.Entity;
using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Implementation
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly AppDbContext _dbContex;
        
        private readonly DbSet<Customer> _dbContexSet;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContex = dbContext;
            _dbContexSet = dbContext.Set<Customer>();
        }

        public DbSet<Customer> Dbset => _dbContexSet;
        public AppDbContext Dbcontex => _dbContex;

        public async Task Delete(Customer entity)
        {
            _dbContex.Customers.Remove(entity);
            await _dbContex.SaveChangesAsync();
        }

        public async Task<Customer> SaveAsync(Customer entity)
        {
            await _dbContex.Customers.AddAsync(entity);
            await _dbContex.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<Customer>> SaveRangeAsync(IEnumerable<Customer> entities)
        {
            await _dbContex.Customers.AddRangeAsync(entities);
            await _dbContex.SaveChangesAsync();

            return entities;
        }
    }
}
