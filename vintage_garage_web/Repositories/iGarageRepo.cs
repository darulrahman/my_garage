using vintage_garage_web.Models;
using vintage_garage_web.Models.Login;

namespace vintage_garage_web.Repositories
{
    public interface iGarageRepo
    {
       
        Task<HttpResponseMessage> GetAllVehicles();
      
        Task<HttpResponseMessage> GetVehiclesById(int id);
  
        Task<HttpResponseMessage> AddVehicle(VehicleReq vehicle);
        Task<HttpResponseMessage> UpdateVehicle(VehicleReq vehicle);
        Task<HttpResponseMessage> DeleteVehicle(int id);
        Task<HttpResponseMessage> GetAllType();
        Task<HttpResponseMessage> GetType(string code);
        Task<HttpResponseMessage> Login(LoginReq log);
        string GetVehicleImage(string vehicleType);
        Task<HttpResponseMessage> GetAllCategories();
    }
}
