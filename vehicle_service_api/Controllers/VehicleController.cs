using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Net;
using vehicle_service_api.DataContexts;
using vehicle_service_api.Models;
using vehicle_service_api.Repositories;

namespace vehicle_service_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;

        private iVehicleRepo _vehicleRepo;
        private iCategoryRepo _catRepo;

        public VehicleController(ILogger<VehicleController> logger, iVehicleRepo vehicleRepo, iCategoryRepo catRepo)
        {
            this._logger = logger;
            this._vehicleRepo = vehicleRepo;
            this._catRepo = catRepo;
        }

        [HttpGet]
        //[Authorize]
        public async Task<List<vehicle>> GetAllVehicle()
        {
            return await this._vehicleRepo.GetAllVehicle();
        }

        [HttpGet("WithCategories")]
        //[Authorize]
        public async Task<List<VehicleRes>> GetAllVehicleWithCategories()
        {
            List<VehicleRes> completeData = new List<VehicleRes>();
            List<vehicle> allVehicle = new List<vehicle>();
            allVehicle = await this._vehicleRepo.GetAllVehicle();
            List<VehicleCategoryMapping> listMapping = new List<VehicleCategoryMapping>();
            listMapping = await this._catRepo.GetCategoriesByVehicles(allVehicle);

            List<int> ids =
                (
                    from c in listMapping
                    select c.categoryId
                    
                ).Distinct().ToList();
            List<Category> categories1 = await _catRepo.GetCategories(ids);

            completeData =
                (
                    from v in allVehicle
                    select new VehicleRes
                    {
                        id = v.id,
                        name = v.name,
                        description = v.description,
                        typeCode = v.typeCode,
                        yearOfManufacture = v.yearOfManufacture
                    }
                ).ToList();

            foreach (VehicleRes v in completeData )
            {
                List<VehicleCategoryMapping> mapp = listMapping.Where(x => x.vehicleId == v.id).ToList();
                List<Category> newMap =
                    (from c in mapp
                     join m in categories1
                     on c.categoryId equals m.id
                     select new Category
                     {
                         id = c.id,
                         description = m.description
                     }).ToList();
                v.categories = newMap;
            }

            return completeData;
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<VehicleRes>> GetVehicle(int id)
        {                       
            vehicle vehicle =  await this._vehicleRepo.GetVehicle(id);

            if (vehicle == null)
                return BadRequest("Vehicle Not Found");

            VehicleRes res = new VehicleRes
            {
                id = vehicle.id,
                description = vehicle.description,
                typeCode = vehicle.typeCode,
                name = vehicle.name,
                yearOfManufacture = vehicle.yearOfManufacture,
                categories = new List<Category>()
            };

            List<VehicleCategoryMapping> listMapping = new List<VehicleCategoryMapping>();
            listMapping = await this._catRepo.GetCategoriesByVehicleId(vehicle.id);

            if(listMapping.Count > 0)
            {
                res.categories.AddRange(
                    listMapping.Select(x => new Category {
                        id = this._catRepo.GetCategoryNonThread(x.categoryId).id,
                        description = this._catRepo.GetCategoryNonThread(x.categoryId).description
                    })
                    );
            }

            return Ok(res);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> InsertVehicle(VehicleReq newVehicle)
        {
            if (newVehicle == null)
                return BadRequest("Data Kosong");

            if(string.IsNullOrWhiteSpace(newVehicle.name))
                return BadRequest("Nama tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(newVehicle.description))
                return BadRequest("Deskripsi tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(newVehicle.typeCode))
                return BadRequest("Tipe Kendaraan tidak boleh kosong");

            if (newVehicle.yearOfManufacture == 0)
                return BadRequest("Tahun Manufacture tidak boleh kosong atau Nol");

            vehicle insertedVehicle = await this._vehicleRepo.InsertVehicle(newVehicle);

            if(insertedVehicle == null)
                return BadRequest("Insert new vehicle failed");

            if(newVehicle.categories != null && newVehicle.categories.Count > 0)
            {
                List<VehicleCategoryMapping> newMapping = new List<VehicleCategoryMapping>();
                newMapping =
                    (
                        from a in newVehicle.categories
                        select new VehicleCategoryMapping
                        {
                            vehicleId = insertedVehicle.id,
                            categoryId = a.id
                        }
                    ).ToList();

                await this._catRepo.InsertVehicleCategoryMapping(newMapping);
            }

            return Ok("Inserted");
        }

        [HttpPut]
        //[Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> UpdateVehicle(VehicleReq newVehicle)
        {
            if (newVehicle == null)
                return BadRequest("Data Kosong");

            if (string.IsNullOrWhiteSpace(newVehicle.name))
                return BadRequest("Nama tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(newVehicle.description))
                return BadRequest("Deskripsi tidak boleh kosong");

            if (string.IsNullOrWhiteSpace(newVehicle.typeCode))
                return BadRequest("Tipe Kendaraan tidak boleh kosong");

            if (newVehicle.yearOfManufacture == 0)
                return BadRequest("Tahun Manufacture tidak boleh kosong atau Nol");

            vehicle existingVehicle = await this._vehicleRepo.GetVehicle(newVehicle.id);

            if (existingVehicle == null)
                return BadRequest("Id Vehicle not found");
            
            await this._catRepo.DeleteVehicleCategories(existingVehicle.id);

            await this._vehicleRepo.UpdateVehicle(newVehicle);

            if (newVehicle.categories != null && newVehicle.categories.Count > 0)
            {
                List<VehicleCategoryMapping> newMapping = new List<VehicleCategoryMapping>();
                newMapping =
                    (
                        from a in newVehicle.categories
                        select new VehicleCategoryMapping
                        {
                            vehicleId = newVehicle.id,
                            categoryId = a.id
                        }
                    ).ToList();

                await this._catRepo.InsertVehicleCategoryMapping(newMapping);
            }
            


            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (id == 0)
                return BadRequest("id cannot be zero");

            vehicle existingVehicle = await this._vehicleRepo.GetVehicle(id);

            if (existingVehicle == null)
                return BadRequest("Id Vehicle not found");

            await this._catRepo.DeleteVehicleCategories(existingVehicle.id);

            await this._vehicleRepo.DeleteVehicle(id);

            return Ok();
        }

    }
}
