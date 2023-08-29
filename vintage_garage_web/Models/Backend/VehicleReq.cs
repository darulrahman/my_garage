using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using vintage_garage_web.Models.Vehicle;

namespace vintage_garage_web.Models
{
    public class VehicleReq
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public string typeCode { get; set; } = string.Empty;
        public int yearOfManufacture { get; set; }
        public List<Category> categories { get; set; } = new List<Category>();
    }
}
