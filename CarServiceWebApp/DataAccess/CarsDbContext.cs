using BusinessEntities;
using FakeDataGenerator;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DataAccess
{
    public class CarsDbContext : DbContext
    {
        public CarsDbContext()
        {
            var dbExists = Database.EnsureCreated();
            if (dbExists)
            {
                /*GenerateRandomObjects();*/
            }

        }

        public CarsDbContext(DbContextOptions options)
            : base(options)
        {
            var dbExists = Database.EnsureCreated();
            if (dbExists)
            {
                var clients = new DataGenerator().GenerateRandomClients(50);
                var materials = new DataGenerator().GenerateRandomMaterials(100);
                var employees = new DataGenerator().GenerateRandomEmployees(50);
                var serviceOptions = new DataGenerator().GenerateRandomOptions();

                ServiceOptions.Add(serviceOptions);
                Clients.AddRange(clients);
                Materials.AddRange(materials);
                Employees.AddRange(employees);
                SaveChanges();
            }
        }


        public DbSet<Client> Clients { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<ServiceOptions> ServiceOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasOne(c => c.Client).WithMany(c => c.Cars).HasForeignKey(c => c.ClientId);
            /*modelBuilder.Entity<Order>().HasMany(o => o.MaterialsUsed)*/
            /*modelBuilder.Entity<OrderLine>().HasOne(o => o.Order).WithMany(o => o.OrderList).HasForeignKey(o => o.OrderId);
            *//*modelBuilder.Entity<InvoiceLine>().HasOne(o => o.Invoice).WithMany(o => o.InvoiceLines).HasForeignKey(o => o.InvoiceId);*//*
            modelBuilder.Entity<CartLine>().HasOne(o => o.Cart).WithMany(o => o.CartLines).HasForeignKey(o => o.CartId);
*/
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString:
               "Server=localhost;Port=5432;User Id=postgres;Password=159874;Database=CarServiceDb;");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }


    }
}
