using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vehicle_service_api.Models;
using vehicle_service_api.Repositories;

namespace vehicle_service_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private iTypeRepo _typeRepo;
        public TypeController(iTypeRepo repo)
        {
            _typeRepo = repo;
        }

        [HttpGet]
        public async Task<List<VehicleType>> GetAllVehicle()
        {
            return await this._typeRepo.GetAllType();
        }

        [HttpGet("{code}")]
        public async Task<VehicleType> GetVehicle(string code)
        {
            return await this._typeRepo.GetType(code);
        }

        [HttpPost]
        public async Task<IActionResult> InsertVehicle(VehicleType newType)
        {
            if (newType == null)
                return BadRequest();

            await this._typeRepo.InsertType(newType);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVehicle(VehicleType newType)
        {
            if (newType == null)
                return BadRequest();

            VehicleType oldType = await this._typeRepo.GetType(newType.typeCode);

            if (oldType == null)
                return BadRequest("Id Vehicle not found");

            await this._typeRepo.UpdateType(newType);

            return Ok();
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteVehicle(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("code cannot be empty");

            VehicleType oldType = await this._typeRepo.GetType(code);

            if (oldType == null)
                return BadRequest("Id Vehicle not found");

            await this._typeRepo.DeleteType(code);

            return Ok();
        }
    }
}
