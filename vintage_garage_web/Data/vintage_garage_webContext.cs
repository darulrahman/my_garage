using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vintage_garage_web.Models;

namespace vintage_garage_web.Data
{
    public class vintage_garage_webContext : DbContext
    {
        public vintage_garage_webContext (DbContextOptions<vintage_garage_webContext> options)
            : base(options)
        {
        }

        public DbSet<vintage_garage_web.Models.VehicleViewModel>? VehicleViewModel { get; set; }
    }
}
