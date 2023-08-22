using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public interface iVehicleRepo
    {
        Task<List<vehicle>> GetAllVehicle();
        Task<vehicle> GetVehicle(int id);
        Task<vehicle> InsertVehicle(vehicle vehicle);
        Task DeleteVehicle(int id);
        Task UpdateVehicle(vehicle vehicle);
    }
}
