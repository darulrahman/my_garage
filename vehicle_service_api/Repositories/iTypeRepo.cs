using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public interface iTypeRepo
    {
        Task<List<VehicleType>> GetAllType();
        Task<VehicleType> GetType(string code);
        Task InsertType(VehicleType type);
        Task DeleteType(string code);
        Task UpdateType(VehicleType type);
    }
}
