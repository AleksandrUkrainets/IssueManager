using IssueManager.Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueManager.Persistance
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserCredential> UserCredentials { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredential>()
                .HasIndex(x => new { x.AppUserId, x.Provider })
                .IsUnique();
        }
    }
}
