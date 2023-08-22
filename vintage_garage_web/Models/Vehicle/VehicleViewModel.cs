﻿using System.ComponentModel;
using vintage_garage_web.Models.Vehicle;

namespace vintage_garage_web.Models
{
    public class VehicleViewModel
    {
        public int id { get; set; }
        [DisplayName("Nama")]
        public string name { get; set; } = string.Empty;
        [DisplayName("Deskripsi")]
        public string? description { get; set; } = string.Empty;
        [DisplayName("Tipe Kendaraan")]
        public string typeCode { get; set; } = string.Empty;
        public string typeName { get; set; } = string.Empty;
        [DisplayName("Tahun Pembuatan")]
        public int yearOfManufacture { get; set; }
        public string imageUrl { get; set; } = string.Empty;
        public List<Category> categories { get; set; } = new List<Category>();
    }
}