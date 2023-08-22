using Microsoft.EntityFrameworkCore;
using vehicle_service_api.DataContexts;
using vehicle_service_api.Models;

namespace vehicle_service_api.Repositories
{
    public class TypeRepo : iTypeRepo
    {
        private readonly VehicleContext _context;
        public TypeRepo(VehicleContext vehicle)
        {
            _context = vehicle;
        }
        public async Task DeleteType(string code)
        {
            VehicleType ty = await _context.vehicleType.FindAsync(code);

            this._context.vehicleType.Remove(ty);

            await this._context.SaveChangesAsync();

            return;
        }

        public async Task<List<VehicleType>> GetAllType()
        {
            return await _context.vehicleType.ToListAsync();
        }

        public async Task<VehicleType> GetType(string code)
        {
            return await _context.vehicleType.FindAsync(code);
        }

        public async Task InsertType(VehicleType type)
        {
            _context.vehicleType.Add(type);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task UpdateType(VehicleType type)
        {
            VehicleType typ = await _context.vehicleType.FindAsync(type.typeCode);

            typ.typeName = type.typeName;

            await this._context.SaveChangesAsync();

            return;
        }
    }
}
