using System.ComponentModel.DataAnnotations;

namespace vehicle_service_api.Models
{
    public class VehicleCategoryMapping
    {
        [Key]
        public int id { get; set; }
        public int vehicleId { get; set; }
        public int categoryId { get; set; }
    }
}
