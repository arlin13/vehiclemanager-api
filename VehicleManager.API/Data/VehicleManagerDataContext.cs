using System.Data.Entity;
using VehicleManager.API.Models;

namespace VehicleManager.API.Data
{
    public class VehicleManagerDataContext : DbContext
    {
        public IDbSet<Vehicle> Vehicles { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Sale> Sales { get; set; }

        public VehicleManagerDataContext()
            : base("VehicleManager")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Vehicle has many sales
            modelBuilder.Entity<Vehicle>()
                .HasMany(vehicle => vehicle.Sales)
                .WithRequired(sale => sale.Vehicle)
                .HasForeignKey(sale => sale.VehicleId);

            // Customer has many sales
            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.Sales)
                .WithRequired(sale => sale.Customer)
                .HasForeignKey(sale => sale.CustomerId);
        }
    }
}