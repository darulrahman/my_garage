using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vehicle_service_api.Models;
using vehicle_service_api.Repositories;

namespace vehicle_service_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private iCategoryRepo _catRepo;
        private iVehicleRepo _vehicleRepo;
        public CategoryController(iCategoryRepo catRepo, iVehicleRepo vehicleRepo)
        {
            _catRepo = catRepo;
            _vehicleRepo = vehicleRepo;
        }

        [HttpGet]
        public async Task<List<Category>> GetAllCategories() { return await _catRepo.GetAllCategories();}

        [HttpGet("{id}")]
        public async Task<Category> GetCategory(int id)
        {
            return await _catRepo.GetCategory(id);
        }

        [HttpGet("GetVehicleCategories/{id}")]        
        public async Task<ActionResult<List<VehicleCategoryMapping>>> GetVehicleCategories(int id)
        {
            if (id == 0)
                return BadRequest("Data Tidak boleh kosong");

            vehicle veh = await _vehicleRepo.GetVehicle(id);

            if (veh == null)
                return BadRequest("vehicle Not Found");

            List<VehicleCategoryMapping> categories = await _catRepo.GetCategoriesByVehicleId(id);

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> InsertCategory(Category category)
        {
            if (category == null)
                return BadRequest("Data Tidak boleh kosong");

            await _catRepo.InsertCategory(category);

            return Ok(category);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            if (category == null)
                return BadRequest("Data Tidak boleh kosong");

            Category oldCat = await _catRepo.GetCategory(category.id);

            if (oldCat == null)
                return BadRequest("Id Category Not Found");
            await _catRepo.UpdateCategory(category);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0)
                return BadRequest("Data Tidak boleh kosong");

            Category oldCat = await _catRepo.GetCategory(id);

            if (oldCat == null)
                return BadRequest("Id Category Not Found");
            await _catRepo.DeleteCategory(id);
            return Ok();
        }
    }
}
