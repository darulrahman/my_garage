using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vehicle_service_api.Models
{
    public class vehicle
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        [Column("type")]        
        public string typeCode { get; set; }= string.Empty;
        public int yearOfManufacture { get; set; }        
    }
}
