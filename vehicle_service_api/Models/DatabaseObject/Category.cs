using System.ComponentModel.DataAnnotations;

namespace vehicle_service_api.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
    }
}
