using System.Collections.Generic;
using System.Data.Entity;

namespace DataAccessLayer
{
    public class DotaDbContext : DbContext
    {
        public DotaDbContext() : base("name=DotaDB")
        {
        }

        // Используем общий тип для EF
        public DbSet<DomainEntity> Heroes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DomainEntity>().ToTable("Heroes");
            modelBuilder.Entity<DomainEntity>().HasKey(h => h.Id);
            modelBuilder.Entity<DomainEntity>().Property(h => h.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<DomainEntity>().Property(h => h.Role).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<DomainEntity>().Property(h => h.Attribute).IsRequired().HasMaxLength(20);

            base.OnModelCreating(modelBuilder);
        }
    }

    // Вспомогательный класс для EF
    public class DomainEntity : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Attribute { get; set; }
        public int Complexity { get; set; }
    }
}