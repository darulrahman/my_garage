using System.ComponentModel.DataAnnotations;

namespace vintage_garage_web.Models.Vehicle
{
    public class Category
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
    }
}
