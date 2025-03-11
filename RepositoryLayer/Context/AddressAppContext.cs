using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class AddressAppContext: DbContext
    {
        public AddressAppContext(DbContextOptions<AddressAppContext> options) : base(options)
        {
        }
        public DbSet<AddressBookEntity> Addresses { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Foreign Key Relationship
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(g => g.User)        // AddressEntity has one User
                .WithMany(u => u.AddressBooks) // UserEntity has many Address
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

    }
}
