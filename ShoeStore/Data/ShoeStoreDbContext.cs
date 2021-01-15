using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStore.Data
{
    public class ShoeStoreDbContext : DbContext
    {
        public ShoeStoreDbContext()
        {
        }

        public ShoeStoreDbContext(DbContextOptions<ShoeStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Shoe> Shoe { get; set; }
        public virtual DbSet<UserCart> UserCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shoe>(entity =>
            {
                entity.Property(e => e.ShoeId).HasColumnName("shoeId");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.ShoeImage)
                    .HasColumnName("shoeImage");

                entity.Property(e => e.ShoeName)
                    .HasColumnName("shoeName")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserCart>(entity =>
            {
                entity.HasKey(e => new { e.ShoeId, e.CartId });

                entity.Property(e => e.ShoeId).HasColumnName("shoeId");

                entity.Property(e => e.CartId)
                    .HasColumnName("cartId")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Shoe)
                    .WithMany(p => p.UserCart)
                    .HasForeignKey(d => d.ShoeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserCart");
            });
        }
    }
}
