using SubscriptionManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Repositories;

namespace SubscriptionManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly ILogger<GenericRepository<T>> _logger;
        protected AppDbContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(
            AppDbContext context,
            ILogger<GenericRepository<T>> logger
            )
        {
            _context = context;
            _logger = logger;

            _dbSet = context.Set<T>();
        }
        public virtual async Task<bool> Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
            {
                var currentValue = (Guid)idProperty.GetValue(entity);
                if (currentValue == Guid.Empty)
                {
                    idProperty.SetValue(entity, Guid.NewGuid());
                }
            }

            await _dbSet.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all entities.");
                throw;
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving entity by ID: {id}");
                throw;
            }
        }

        public virtual async Task<bool> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                return await SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the entity.");
                throw;
            }
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to the database.");
                throw;
            }
        }
    }
}
