using System.Collections.Generic;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class DotaDbContext : DbContext
    {
        public DotaDbContext() : base("DotaDB")
        {
        }

        public DbSet<Hero> Heroes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>().ToTable("Heroes");
            modelBuilder.Entity<Hero>().HasKey(h => h.Id);
            modelBuilder.Entity<Hero>().Property(h => h.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Hero>().Property(h => h.Role).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Hero>().Property(h => h.Attribute).IsRequired().HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }
}