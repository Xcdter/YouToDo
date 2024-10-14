using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using WebTest2.Models;
using Microsoft.AspNetCore.Identity;
using System.Web.Helpers;

namespace YouToDo.Repositories
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
