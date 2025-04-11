using IssueManager.Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace IssueManager.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<UserCredential> UserCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredential>()
                .HasIndex(x => new { x.AppUserId, x.Provider })
                .IsUnique();
        }
    }
}
