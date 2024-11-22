using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using WebTest2.Models;
using Microsoft.AspNetCore.Identity;
using System.Web.Helpers;
using YouToDo.Models;

namespace YouToDo.Repositories
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TaskModel> Tasks { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.Property(e => e.Name).HasColumnName("Name");

                entity.Property(e => e.Email).HasColumnName("Email");

                entity.Property(e => e.Password).HasColumnName("Password");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            });

            modelBuilder.Entity<TaskModel>(entity =>
            {
                entity.ToTable("Tasks");

                entity.Property(e => e.Id).HasColumnName("task_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.DueDate).HasColumnName("due_date");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.Tags).HasColumnName("tags");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");

                entity.Property(e => e.Id).HasColumnName("project_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

                entity.Property(e => e.DueDate).HasColumnName("due_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
