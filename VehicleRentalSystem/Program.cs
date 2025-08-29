using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleRentalSystem.Template;

namespace VehicleRentalSystem.Template
{
    public interface IRentable
    {
        public void Rent(DateTime start, DateTime end);
        public void ReturnVehicle();
    }
    public class Vehicle: IRentable
    {
        [Key]
        public int VehicleId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int DailyRate { get; set; }
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID: {VehicleId}, Model: {Model}, Rate: {DailyRate}, Available: {IsAvailable}");
        }
        public void Rent(DateTime start, DateTime end)
        {
            if (!IsAvailable)
            {
                Console.WriteLine("Ops, already rented"); return;
            }
            IsAvailable = false;
            Console.WriteLine($"{Model} rented from {start:yyyy-MM-dd} to {end:yyyy-MM-dd}");
        }
        public void ReturnVehicle()
        {
            IsAvailable = true;
            Console.WriteLine($"{Model} Returned, Thanks");
        }
    }
    public class Car : Vehicle
    {
        public int NumOfDoors { get; set; }
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"No. of Doors :{NumOfDoors}");
        }
        /*
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
        }*/
    }
    public class Bike : Vehicle
    {
        public int EngineCapacity { get; set; }
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Engine Capacity : {EngineCapacity}");
        }
        /*public void Rent(DateTime start, DateTime end)
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
        }*/
    }
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int LicenseNumber { get; set; }
    }
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }
        public Vehicle Vehicle { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = MyDatabase.db");
            // if this being written then no need of appsettings.json nd
            // simply we've instantiate RentalDbContext, ef core will create .db file in bin/Debug/net8.0/
        }
        /*
         in case u wanna load json file then u have to specify below in RentalDbContext:
            
             private readonly IConfiguration _configuration;

             public RentalDbContext(IConfiguration configuration)
             {
                _configuration = configuration;
             }
        */
    }
}

namespace VehicleRentalSystem
{
    class Program
    {
        static void Main(string[] args)
        {

            /*IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            IConfiguration is a .NET interface used to read configuration settings, 
            such as those in appsettings.json
            */

            // Initialised DBcontext
            using var context = new RentalDbContext();
            context.Database.EnsureCreated();

            Console.WriteLine("Vehicle Rental System");
            Console.WriteLine("1. Add Vehicle");
            Console.WriteLine("2. Register Customer");
            Console.WriteLine("3. Rent Vehicle");
            Console.WriteLine("4. Return Vehicle");
            Console.WriteLine("5. Show Available Vehicles");
            Console.WriteLine("6. Show All Rentals");
            Console.WriteLine("7. Exit");

            while (true)
            {
                Console.WriteLine();
                Console.Write("Choose an option: ");

                string option = Console.ReadLine();

                Console.WriteLine();

                switch (option)
                {

                    case "1":
                        AddVehicle(context); break;
                    case "2":
                        RegisterCustomer(context); break;
                    case "3":
                        RentVehicle(context); break;
                    case "4":
                        ReturnVehicle(context); break;
                    case "5":
                        ShowAvailableVehicles(context); break;
                    case "6":
                        ShowAllRentals(context); break;
                    case "7": return;

                    default: Console.WriteLine("Invalid choice, Try again !"); break;
                }
            }
        }
        static void AddVehicle(RentalDbContext context)
        {

            Console.WriteLine("Enter the type of Vehicle, 1 for CAR & 2 for BIKE");
            string type = Console.ReadLine();

            Console.WriteLine("Enter the Vehicle Id");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Id");
                return;
            }
            Console.WriteLine("Enter the Brand");
            string brand = Console.ReadLine();

            Console.WriteLine("Enter the Model");
            string model = Console.ReadLine();

            Console.WriteLine("Enter the Daily Rate");

            if (!int.TryParse(Console.ReadLine(), out int rate))
            {
                Console.WriteLine("Invalid Rate");
                return;
            }

            if (type == "1")
            {
                Console.WriteLine("Enter the Number of Doors");

                if (!int.TryParse(Console.ReadLine(), out int doors))
                {
                    Console.WriteLine("Invalid No. of Doors");
                    return;
                }
                Car car = new Car()
                {
                    VehicleId = id,
                    Model = model,
                    Brand = brand,
                    DailyRate = rate,
                    NumOfDoors = doors
                };
                context.Vehicles.Add(car);
                context.SaveChanges();
                //var v = context.Vehicles.ToList();
                Console.WriteLine("Car added");
            }

            else if (type == "2")
            {

                Console.WriteLine("Enter the Engine Capacity");

                if (!int.TryParse(Console.ReadLine(), out int eCap))
                {
                    Console.WriteLine("Invalid Engine Capacity");
                    return;
                }

                Bike bike = new Bike()
                {
                    VehicleId = id,
                    Model = model,
                    Brand = brand,
                    DailyRate = rate,
                    EngineCapacity = eCap
                };
                context.Vehicles.Add(bike);
                context.SaveChanges();
                Console.WriteLine("Bike added");
            }
            else
            {
                Console.WriteLine("Invalid Vehicle Type");
            }
        }
        static void RegisterCustomer(RentalDbContext context)
        {
            Console.WriteLine("Enter Customer ID");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Customer ID, Try Again !");
            }

            Console.WriteLine("Enter your Name");
            string name = Console.ReadLine();

            Console.WriteLine("Enter License Number");
            if (!int.TryParse(Console.ReadLine(), out int licenseNo))
            {
                Console.WriteLine("Invalid License Number, Try Again !");
            }

            Customer customer = new Customer
            {
                CustomerId = id,
                Name = name,
                LicenseNumber = licenseNo
            };
            context.Customers.Add(customer);
            context.SaveChanges();
            Console.WriteLine("Customer Registered");

        }
        static void RentVehicle(RentalDbContext context)
        {
            Console.WriteLine("Enter the Vehicle Id");

            if (!int.TryParse(Console.ReadLine(), out int vId))
            {
                Console.WriteLine("Invalid Id");
                return;
            }

            Console.WriteLine("Enter the Customer Id");

            if (!int.TryParse(Console.ReadLine(), out int cID))
            {
                Console.WriteLine("Invalid Id");
                return;
            }

            Console.WriteLine("Enter the Start Date (yyyy-MM-dd)");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
            {
                Console.WriteLine("Invalid format");
                return;
            }

            Console.WriteLine("Enter the End Date (yyyy-MM-dd)");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
            {
                Console.WriteLine("Invalid format");
                return;
            }

            Vehicle vehicle = context.Vehicles.Where(v => v.VehicleId == vId).FirstOrDefault();
            Customer customer = context.Customers.Where(c => c.CustomerId == cID).FirstOrDefault();

            if (vehicle == null || customer == null)
            {
                Console.WriteLine("Vehicle or customer not found");
                return;
            }
            //Console.WriteLine($"Is IRentable: {vehicle is IRentable}"); // says false cuz vehicle object doesn implement IRentable interface
            //Console.WriteLine($"Is Available: {vehicle.IsAvailable}");

            if (vehicle.IsAvailable)
            {
                vehicle.Rent(start, end);

                Rental rental = new Rental()
                {
                    //RentalId = context.Rentals.Count() + 1, query entire table
                    //but EF handles Id generation automatically
                    Vehicle = vehicle,
                    Customer = customer,
                    StartDate = start,
                    EndDate = end,
                    TotalAmount = (end - start).Days * vehicle.DailyRate
                };
                context.Rentals.Add(rental);
                context.SaveChanges();
                Console.WriteLine($"Rented {vehicle.Model} to {customer.Name} for ${rental.CalculateTotal()}");
            }

            else Console.WriteLine("Vehicle not available or not rentable");
        }
        static void ReturnVehicle(RentalDbContext context)
        {

            Console.WriteLine("Enter the Vehicle ID");
            if (!int.TryParse(Console.ReadLine(), out int vId))
            {
                return;
            }

            //Vehicle vehicle = (Vehicle)context.Vehicles.Where(v => v.VehicleId == vId).ToList();
            /* 
               -ToList() returns a List<Vehicle>, not a single Vehicle
               -You can't cast a list to a single object
             
             
             */
            Vehicle vehicle = context.Vehicles.Where(v => v.VehicleId == vId).FirstOrDefault();
            /*
             Get the first record that matches the condition
             If no records match, return default (which is null for reference types like Vehicle)
             */

            if (vehicle == null)
            {
                Console.WriteLine("Vehicle not found or not rentable");
                return;
            }

            if (!vehicle.IsAvailable)
            {
                vehicle.ReturnVehicle();
                context.SaveChanges();
            }
        }
        static void ShowAvailableVehicles(RentalDbContext context)
        {

            foreach (var vehicle in context.Vehicles.ToList())
            {

                if (vehicle.IsAvailable)
                {
                    vehicle.DisplayInfo();
                }

            }
        }
        static void ShowAllRentals(RentalDbContext context)
        {
            /*
            //List<Vehicle> vehicles = context.Vehicles.ToList();
            //List<Customer> customers = context.Customers.ToList();
            
            -Change Tracker Cache: EF keeps loaded entities in memory
            -EF uses cached data instead of hitting the database
            -Navigation properties get resolved automatically from cache
            -that's y rental.Vehicle?.Model will show data
            */

            foreach (var rental in context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.Customer) //Include() loads the data
                .ToList())
            //in case i won't add INCLUDE Entity Framework doesn't auto load navigation properties
            // referred this article www.learnentityframeworkcore.com/dbset/querying-data
            {

                Console.WriteLine($"Rental ID: {rental.RentalId}, Vehicle: {rental.Vehicle.Model}, Customer: {rental.Customer.Name}, Start: {rental.StartDate:yyyy-MM-dd}, End: {rental.EndDate:yyyy-MM-dd}, Total: ${rental.TotalAmount}");

            }
        }

    }
}