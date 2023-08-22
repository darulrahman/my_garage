using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public interface iCategoryRepo
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<List<Category>> GetCategories(List<int> ids);
        Task<List<VehicleCategoryMapping>> GetCategoriesByVehicleId(int id);
        Task<List<VehicleCategoryMapping>> GetCategoriesByVehicles(List<vehicle> vehicles);
        Task InsertCategory(Category type);
        Task InsertVehicleCategoryMapping(List<VehicleCategoryMapping> mapping);
        Task DeleteCategory(int id);
        Task DeleteVehicleCategories(int id);
        Task UpdateCategory(Category type);
    }
}
