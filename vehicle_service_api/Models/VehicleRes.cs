namespace vehicle_service_api.Models
{
    public class VehicleRes : vehicle
    {
        public List<Category> categories { get; set; } = new List<Category>();
    }
}
