using Microsoft.EntityFrameworkCore;
using TwseApp.API.Entities;
using TwseApp.API.Models;

namespace TwseApp.API.Data
{
    public class TwseDbContext : DbContext
    {
        public TwseDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<Headquarters> Headquarters { get; set; }
        public DbSet<Branch> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Headquarters)
                .WithMany()
                .HasForeignKey(b => b.Code)
                .IsRequired();
        }
    }
}
