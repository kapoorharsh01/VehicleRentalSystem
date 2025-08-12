using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalSystem.Template;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace VehicleRentalSystem.Template 
{
    public interface IRentable
    {
        public void Rent(DateTime start, DateTime end);
        public void ReturnVehicle();

    }

    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Model { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public int DailyRate { get; set; }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {VehicleId}, Model: {Model}, Rate: {DailyRate}, Available: {IsAvailable}");
        }
    }

    public class Car : Vehicle, IRentable
    {
        public int NumOfDoors { get; set; }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"No. of Doors :{NumOfDoors}");
        }

        public void Rent(DateTime start, DateTime end)
        {

            if (!IsAvailable)
            {
                Console.WriteLine("Ops, already rented"); return;
            }
            IsAvailable = false;
            Console.WriteLine($"Car rented from {start:yyyy-MM-dd} to {end:yyyy-MM-dd}");

        }
        public void ReturnVehicle()
        {
            IsAvailable = true;
            Console.WriteLine("Car Returned, Thanks");
        }
    }

    public class Bike : Vehicle, IRentable
    {
        public int EngineCapacity { get; set; }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Engine Capacity : {EngineCapacity}");
        }
        public void Rent(DateTime start, DateTime end)
        {

            if (!IsAvailable)
            {
                Console.WriteLine("Ops, already rented"); return;
            }

            IsAvailable = false;
            Console.WriteLine($"Bike rented from {start:yyyy-MM-dd} to {end:yyyy-MM-dd}");
        }

        public void ReturnVehicle()
        {
            IsAvailable = true;
            Console.WriteLine("Bike Returned, Thanks");
        }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int LicenseNumber { get; set; }

    }

    public class Rental
    {

        public int RentalId { get; set; }
        public Vehicle Vehicle { get; set; } // did gpt for understanding how to pass object in getters & setters
        public Customer Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalAmount { get; set; }

        public decimal CalculateTotal()
        {
            return (EndDate - StartDate).Days * Vehicle.DailyRate;
        }
    }
    public class RentalDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

    }
}


namespace VehicleRentalSystem
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}