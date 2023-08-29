using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Collections.Generic;
using System.IO;
using System.Net;
using vintage_garage_web.Models;
using vintage_garage_web.Models.Vehicle;
using vintage_garage_web.Repositories;

namespace vintage_garage_web.Controllers
{
    public class VehicleController : Controller
    {
        private iGarageRepo _repo;
        public VehicleController(iGarageRepo repo)
        {
            _repo = repo;
        }        
        public async Task<IActionResult> Index()
        {
            List<VehicleViewModel> listVeh = new List<VehicleViewModel>();

            Task<HttpResponseMessage> resVehicle = _repo.GetAllVehicles();
            Task<HttpResponseMessage> resType = _repo.GetAllType();

            await Task.WhenAll(resVehicle, resType);

            if(resVehicle.Result.IsSuccessStatusCode && resType.Result.IsSuccessStatusCode)
            {
                string strVeh = resVehicle.Result.Content.ReadAsStringAsync().Result;
                string strType = resType.Result.Content.ReadAsStringAsync().Result;
                List<VehicleViewModel> tmpVeh = new List<VehicleViewModel>();
                tmpVeh = JsonConvert.DeserializeObject<List<VehicleViewModel>>(strVeh);
                List<TypeViewModel> tmpType = JsonConvert.DeserializeObject<List<TypeViewModel>>(strType);
                tmpVeh.ForEach(x => {
                    x.typeName = tmpType.Where(z => z.typeCode == x.typeCode).Select(z => z.typeName).FirstOrDefault();
                    x.imageUrl = _repo.GetVehicleImage(x.typeCode);
                    });
                listVeh = tmpVeh;
            }


            return View(listVeh);
        }

        public async Task<IActionResult> Create()
        {
            VehicleViewModel model = new VehicleViewModel();
            model.Action = "create";
            
            List<TypeViewModel> list = this.GetType();

            ViewData["vehicleType"] = new SelectList(list, "typeCode", "typeName");

            List<Category> cats = this.GetCategory();
            ViewBag.ItemsBag = new MultiSelectList(cats, "id", "description");

            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> Create(VehicleViewModel vehicle)
        {
            string errMessage = "";
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                List<Category> SelectedCat = new List<Category>();
                if (vehicle.SelectedCategory.Count > 0)
                    SelectedCat.AddRange(
                        vehicle.SelectedCategory.Select(x => new Category
                        {
                            id = x,
                            description = ""
                        })
                        );
                VehicleReq vehicleReq = new VehicleReq
                {
                    id = vehicle.id,
                    name = vehicle.name,
                    description = vehicle.description,
                    categories = SelectedCat,
                    typeCode = vehicle.typeCode,
                    yearOfManufacture = vehicle.yearOfManufacture
                };
                response = await this._repo.AddVehicle(vehicleReq);
                
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Vehicle");
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    errMessage = await response.Content.ReadAsStringAsync();
                }
                errMessage = string.IsNullOrEmpty(errMessage) ? "Gagal Insert Data" : errMessage;

            }
            else
            {
                errMessage = "Data Tidak Valid";
            }

            TempData["Error"] = errMessage;
           
            List<TypeViewModel> list = this.GetType();

            ViewData["vehicleType"] = new SelectList(list, "typeCode", "typeName");

            List<Category> cats = this.GetCategory();
            ViewBag.ItemsBag = new MultiSelectList(cats, "id", "description");

            return View(vehicle);
        }

        public async Task<IActionResult> Edit(int id)
        {
            VehicleViewModel model = new VehicleViewModel();

            HttpResponseMessage response = await this._repo.GetVehiclesById(id);
            
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                VehicleViewModel tmpModel = JsonConvert.DeserializeObject<VehicleViewModel>(result);

                List<TypeViewModel> list = this.GetType();

                ViewData["vehicleType"] = new SelectList(list, "typeCode", "typeName");

                

                tmpModel.typeName = list.Where(z => z.typeCode == tmpModel.typeCode).Select(x => x.typeName).FirstOrDefault();

                int[] asd = new int[] { 0 };

                if (tmpModel.categories.Count > 0)
                {
                    asd = tmpModel.categories.Select(x => x.id).ToArray();
                }

                List<Category> cats = this.GetCategory();
                ViewBag.ItemsBag = new MultiSelectList(cats, "id", "description", asd);

                model = tmpModel;
                model.Action = "Edit";
            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleViewModel vehicle)
        {
            List<Category> SelectedCat = new List<Category>();
            if (vehicle.SelectedCategory.Count > 0)
                SelectedCat.AddRange(
                    vehicle.SelectedCategory.Select(x => new Category
                    {
                        id = x,
                        description = ""
                    })
                    );
            VehicleReq vehicleReq = new VehicleReq
            {
                id = vehicle.id,
                name = vehicle.name,
                description = vehicle.description,
                categories = SelectedCat,
                typeCode = vehicle.typeCode,
                yearOfManufacture = vehicle.yearOfManufacture
            };
            HttpResponseMessage response = await this._repo.UpdateVehicle(vehicleReq);
            string errMessage = "";

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Vehicle");
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                errMessage = await response.Content.ReadAsStringAsync();
            }
            errMessage = string.IsNullOrEmpty(errMessage) ? "Gagal Update Data" : errMessage;
            TempData["Error"] = errMessage;

            List<TypeViewModel> list = this.GetType();

            ViewData["vehicleType"] = new SelectList(list, "typeCode", "typeName");

            List<Category> cats = this.GetCategory();
            ViewBag.ItemsBag = new MultiSelectList(cats, "id", "description");

            return View("Create", vehicle);
        }

        public async Task<IActionResult> Delete(int id)
        {
            VehicleViewModel model = new VehicleViewModel();
            HttpResponseMessage response = await this._repo.DeleteVehicle(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Vehicle");
            }
            return View();
        }

        private List<Category> GetCategory()
        {
            HttpResponseMessage listCat = _repo.GetAllCategories().GetAwaiter().GetResult();
            List<Category> cats = new List<Category>();
            if (listCat.IsSuccessStatusCode)
            {
                string strCat = listCat.Content.ReadAsStringAsync().Result;
                cats = JsonConvert.DeserializeObject<List<Category>>(strCat);
            }

            return cats;
        }

        private List<TypeViewModel> GetType()
        {
            HttpResponseMessage listCat = _repo.GetAllType().GetAwaiter().GetResult();
            List<TypeViewModel> cats = new List<TypeViewModel>();
            if (listCat.IsSuccessStatusCode)
            {
                string strCat = listCat.Content.ReadAsStringAsync().Result;
                cats = JsonConvert.DeserializeObject<List<TypeViewModel>>(strCat);
            }

            return cats;
        }
    }
}
