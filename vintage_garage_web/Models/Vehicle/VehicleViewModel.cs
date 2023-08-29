using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vintage_garage_web.Models.Vehicle;

namespace vintage_garage_web.Models
{
    public class VehicleViewModel
    {
        public string Action { get; set; }
        public int id { get; set; }
        [DisplayName("Nama")]
        public string name { get; set; } = string.Empty;
        [DisplayName("Deskripsi")]
        public string? description { get; set; } = string.Empty;
        [DisplayName("Tipe Kendaraan")]
        [Required]
        public string typeCode { get; set; } = string.Empty;
        public string typeName { get; set; } = string.Empty;
        [DisplayName("Tahun Pembuatan")]
        public int yearOfManufacture { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public List<Category> categories { get; set; } = new List<Category>();
        public List<int> SelectedCategory { get; set; }
    }
}
