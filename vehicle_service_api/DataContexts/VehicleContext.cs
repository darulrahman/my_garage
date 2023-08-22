using Microsoft.EntityFrameworkCore;
using vehicle_service_api.Models;

namespace vehicle_service_api.DataContexts
{
    public class VehicleContext: DbContext
    {
        public VehicleContext(DbContextOptions<VehicleContext> option): base(option)
        {
            
        }

        public DbSet<vehicle> vehicle { get; set; }
        public DbSet<VehicleType> vehicleType { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<VehicleCategoryMapping> VehicleCategories { get; set; }
    }
}
