using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

namespace ConfigManagerStend.Domain
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<Stand> Stands { get; set; }
        internal DbSet<ConfigStatus> ConfigStatuses { get; set; }
        internal DbSet<ConfigStend> ConfigStends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = ..\\ConfMangerStend.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConfigStatus>().HasData(new PdConfigStatus().ConfigStatuses);
        }
    }
}
