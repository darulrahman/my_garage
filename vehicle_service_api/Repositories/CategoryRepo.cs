using Microsoft.EntityFrameworkCore;
using vehicle_service_api.DataContexts;
using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public class CategoryRepo : iCategoryRepo
    {
        private readonly VehicleContext _context;
        public CategoryRepo(VehicleContext vehicle)
        {
            _context = vehicle;
        }
        public async Task DeleteCategory(int id)
        {
            Category ty = await _context.category.FindAsync(id);

            this._context.category.Remove(ty);

            await this._context.SaveChangesAsync();

            return;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.category.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _context.category.FindAsync(id);
        }

        public async Task<List<VehicleCategoryMapping>> GetCategoriesByVehicleId(int id)
        {
            return await _context.VehicleCategories.Where(x => x.vehicleId == id).ToListAsync();
        }

        public async Task InsertCategory(Category type)
        {
            _context.category.Add(type);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task UpdateCategory(Category type)
        {
            Category typ = await _context.category.FindAsync(type.id);

            typ.description = type.description;

            await this._context.SaveChangesAsync();

            return;
        }

        public async Task<List<VehicleCategoryMapping>> GetCategoriesByVehicles(List<vehicle> vehicles)
        {
            var listOfVehicleId = vehicles.Select(r => r.id);
            return await _context.VehicleCategories.Where(x => listOfVehicleId.Contains(x.vehicleId)).ToListAsync();
        }

        public async Task InsertVehicleCategoryMapping(List<VehicleCategoryMapping> mapping)
        {
            foreach (var vehicleCategoryMapping in mapping)
            { 
                _context.VehicleCategories.Add(vehicleCategoryMapping);
                _context.SaveChanges();
            }
            return;
        }

        public async Task DeleteVehicleCategories(int id)
        {
            var groups = _context.VehicleCategories.Where(g => g.vehicleId == id);
            _context.VehicleCategories.RemoveRange(groups);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task<List<Category>> GetCategories(List<int> ids)
        {            
            return await _context.category.Where(x => ids.Contains(x.id)).ToListAsync();
        }
    }
}
