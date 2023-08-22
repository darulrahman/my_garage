namespace vehicle_service_api.Models
{
    public class VehicleReq:vehicle
    {
        public List<Category> categories { get; set; } = new List<Category>();
    }
}
