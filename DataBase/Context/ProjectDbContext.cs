﻿using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Utils.Enums;

namespace DataBase.Context
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectManager;Username=postgres;Password=1q2w3e");
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasConversion(
                          v => v.ToString(),
                          v => (UserRole.UserRoleEnum)Enum.Parse(typeof(UserRole.UserRoleEnum), v));
            });

            modelBuilder.Entity<ProjectEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description);
                entity.Property(e => e.CreatedDate).IsRequired();
                
                entity.HasOne<UserEntity>(e => e.Asignee)
                    .WithMany(u => u.Projects)
                    .HasForeignKey(e => e.AsigneeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description);
                entity.Property(e => e.Finished).HasDefaultValue(false);

                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<UserEntity>(e => e.Asignee)
                    .WithMany(u => u.Tasks)
                    .HasForeignKey(e => e.AsigneeID)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
