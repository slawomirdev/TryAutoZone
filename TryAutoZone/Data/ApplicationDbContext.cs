﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TryAutoZone.Models;

namespace TryAutoZone.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TryAutoZone.Models.Car>? Car { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(car => car.FuelConsumption)
                .HasPrecision(5, 2); 
        }

        public DbSet<TryAutoZone.Models.FavoriteCar>? FavoriteCar { get; set; }
    }
}