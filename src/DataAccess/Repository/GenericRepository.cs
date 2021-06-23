using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YNews.DataAccess.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            await DeleteAsync(entityToDelete);
        }

        public virtual async Task DeleteAsync(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
            await context.SaveChangesAsync();
        }
    }
}