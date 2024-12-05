using Microsoft.EntityFrameworkCore;
using TaskManagement.Repository.Entities;
using TaskManagement.Repository.Interfaces;

namespace TaskManagement.Repository
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
        : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }

        // Implement the interface methods
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
