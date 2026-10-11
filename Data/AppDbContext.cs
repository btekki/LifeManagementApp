using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LifeManagementApp.Models;

namespace LifeManagementApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> Todos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.ToTable("TodoItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Priority)
                    .HasDefaultValue(3);
                entity.Property(e => e.IsDone)
                    .HasDefaultValue(false);
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

            });
        }
    }
}
