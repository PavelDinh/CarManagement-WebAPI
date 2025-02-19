using CarOwnerManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarOwnerManagement
{
    public class CarManagementDbContext(DbContextOptions<CarManagementDbContext> options) : DbContext(options)
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasData(new Car
                {
                    Id = 1,
                    Name = "Tesla Model S",
                    Description = "A high-performance luxury electric sedan with impressive acceleration, long range, and Autopilot capabilities.",
                    FuelType = Data.FuelType.Electric,
                },
                new Car
                {
                    Id = 2,
                    Name = "Ford F-150",
                    Description = "America’s best-selling full-size pickup truck, known for its durability, towing capacity, and versatility.",
                    FuelType = Data.FuelType.Gasoline,
                },
                new Car
                {
                    Id = 3,
                    Name = "Chevrolet Silverado 1500",
                    Description = "A powerful and reliable diesel pickup truck designed for heavy-duty work and off-road capability.",
                    FuelType = Data.FuelType.Diesel,
                },
                new Car
                {
                    Id = 4,
                    Name = "Porsche 911",
                    Description = "A luxury electric sedan with a spacious interior, autonomous driving, and a state-of-the-art infotainment system.",
                    FuelType = Data.FuelType.Gasoline,
                },
                new Car
                {
                    Id = 5,
                    Name = "BMW i4",
                    Description = "A sporty and luxurious electric sedan combining BMW’s signature driving dynamics with zero emissions.",
                    FuelType = Data.FuelType.Electric
                });

            modelBuilder.Entity<Owner>()
                .HasData(new Owner
                {
                    Id = 1,
                    Name = "Neo Anderson",
                },
                new Owner
                {
                    Id = 2,
                    Name = "Darth Wader",
                },
                new Owner
                {
                    Id = 3,
                    Name = "Gandalf Grey",
                });

            modelBuilder.Entity<Car>()
                .HasMany(c => c.Owners)
                .WithMany(o => o.Cars)
                .UsingEntity(j => j.ToTable("CarOwners").HasData(
                    new { CarsId = 1, OwnersId = 3 },
                    new { CarsId = 2, OwnersId = 2 },
                    new { CarsId = 3, OwnersId = 1 },
                    new { CarsId = 4, OwnersId = 2 },
                    new { CarsId = 4, OwnersId = 3 }
                    ));
        }
    }
}
