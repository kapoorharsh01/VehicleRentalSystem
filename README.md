# Vehicle Rental System

## Objective:
Upgrade the console-based Vehicle Rental System to persist data in a local database, ensuring that information is retained even after the application is closed.

## Guidelines: 
  - Use SQLite as the local database.
  - Use Entity Framework Core for ORM.
  - Continue with the current console application structure.

## Tasks:
  - Add Database Context
     * Create a RentalDbContext class inheriting from DbContext.
     * Define DbSet properties for Vehicles, Customers, and Rentals.
       
  - Update Entity Classes
     * Add EF Core annotations or use fluent API configurations as needed.
       
  - Configure SQLite Database
     * Update appsettings.json for the SQLite database file.
         
  - CRUD Operations
     * Replace in-memory lists with database queries for:
          - Adding vehicles/customers
          - Renting/returning vehicles
          - Listing available vehicles
          - Displaying rentals
