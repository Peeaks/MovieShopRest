﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DLL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DLL.Contexts {
    public class MovieShopContext : IdentityDbContext<Customer> {
        public MovieShopContext() : base("MovieDB") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasMany(a => a.Movies).WithMany();

            modelBuilder.Entity<Cart>().HasMany(a => a.Movies).WithMany();

            modelBuilder.Entity<Cart>().HasOptional(x => x.PromoCode);
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public static MovieShopContext Create() {
            return new MovieShopContext();
        }
    }
}