using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using Microsoft.EntityFrameworkCore;

namespace ConfigManagerStend.Domain
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<ConfigStatus> ConfigStatuses { get; set; }
        internal DbSet<Config> Configs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ConfMangerStend.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConfigStatus>().HasData(new PdConfigStatus().ConfigStatuses);
        }
    }
}
