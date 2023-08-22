using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vehicle_service_api.Models
{
    public class VehicleType
    {
        [Key]
        [Column("code")]
        public string typeCode { get; set; } = string.Empty;
        [Column("name")]
        public string typeName { get; set; } = string.Empty;
    }
}
