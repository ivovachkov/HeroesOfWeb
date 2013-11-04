using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext()
            : base("StoreContextDb")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<ItemCategory> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Hero> Heros { get; set; }

        public DbSet<Monster> Monsters { get; set; }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Battle> Battles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>()
                .HasOptional(t => t.User)
                .WithOptionalPrincipal();

            modelBuilder.Entity<User>()
                .HasOptional(t => t.Hero)
                .WithOptionalPrincipal();

            base.OnModelCreating(modelBuilder);
        }
    }
}
