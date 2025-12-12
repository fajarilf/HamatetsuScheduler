using Microsoft.EntityFrameworkCore;

namespace HamatetsuScheduler.Api.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Dbset { get; }
        Task<T> SaveAsync(T entity);
        Task<IEnumerable<T>> SaveRangeAsync (IEnumerable<T> entities);
        Task Delete(T entity);
    }
}
