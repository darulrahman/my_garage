using Microsoft.EntityFrameworkCore;
using vehicle_service_api.DataContexts;
using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public class VehicleRepo : iVehicleRepo
    {
        private readonly VehicleContext _context;
        public VehicleRepo(VehicleContext vehicle)
        {
            _context = vehicle;
        }

        public async Task DeleteVehicle(int id)
        {
            vehicle veh = await _context.vehicle.FindAsync(id);

            this._context.vehicle.Remove(veh);

            await this._context.SaveChangesAsync();

            return;
        }

        public async Task<List<vehicle>> GetAllVehicle()
        {           
            return await _context.vehicle.FromSql($"SELECT * FROM vehicle").ToListAsync();
        }

        public async Task<vehicle> GetVehicle(int id)
        {
            return await _context.vehicle.FindAsync(id);
        }

        public async Task<vehicle> InsertVehicle(vehicle vehicle)
        {
            _context.vehicle.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task UpdateVehicle(vehicle vehicle)
        {
            vehicle veh = await _context.vehicle.FindAsync(vehicle.id);

            veh.name = vehicle.name;
            veh.description = vehicle.description;
            veh.typeCode = vehicle.typeCode;
            veh.yearOfManufacture = vehicle.yearOfManufacture;

            await this._context.SaveChangesAsync();

            return;
        }
    }
}
