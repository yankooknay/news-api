using Microsoft.EntityFrameworkCore;
using YNewsApi.Entities.Entity;

namespace YNews.DataAccess.Db
{
    public class NewsCacheContext : DbContext
    {
        public NewsCacheContext(DbContextOptions<NewsCacheContext> options) : base(options) { }

        public virtual DbSet<NewsItem> NewsItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.ToTable("NewsItem");
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Ignore(e => e.Dead);
            });
        }
    }
}
