using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Repository.Interfaces
{
    //Interface for DbContext class
    public interface ITaskDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
