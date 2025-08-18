﻿using ConfigManagerStend.Domain.Entities;
using ConfigManagerStend.Domain.Predefineds;
using Microsoft.EntityFrameworkCore;

namespace ConfigManagerStend.Domain
{
    internal class AppDbContext : DbContext
    {
        internal DbSet<ConfigStatus> ConfigStatuses { get; set; }
        internal DbSet<Config> Configs { get; set; }
        internal DbSet<TeamProject> TeamProjects { get; set; }
        internal DbSet<BuildDefinition> BuildDefinitions { get; set; }
        internal DbSet<ConfigStend> ConfigStends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ConfMangerStend.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConfigStatus>().HasData(new PdConfigStatus().ConfigStatuses);
            modelBuilder.Entity<TeamProject>().HasData(new PdTeamProject().Projects);
            modelBuilder.Entity<BuildDefinition>().HasData(new PdBuildDefinition().Repos);
        }
    }
}
