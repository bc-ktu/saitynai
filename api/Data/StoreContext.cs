using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using api.Entities;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .IsRequired(false);

            modelBuilder
                .Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}