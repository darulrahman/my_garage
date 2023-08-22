﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Collections.Generic;
using vintage_garage_web.Models;
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
                List<VehicleViewModel> tmpVeh = JsonConvert.DeserializeObject<List<VehicleViewModel>>(strVeh);
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
            HttpResponseMessage listType = await _repo.GetAllType();
            List<TypeViewModel> list = new List<TypeViewModel>();
            if (listType.IsSuccessStatusCode)
            {

                string inserted = listType.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<TypeViewModel>>(inserted);
                ViewData["vehicleType"] = new SelectList(list, "typeCode", "typeName");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleViewModel vehicle)
        {
            HttpResponseMessage response = await this._repo.AddVehicle(vehicle);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Vehicle");

            TempData["Error"] = "Gagal insert data";
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            VehicleViewModel model = new VehicleViewModel();

            Task<HttpResponseMessage> response = this._repo.GetVehiclesById(id);
            Task<HttpResponseMessage> listType = _repo.GetAllType();

            await Task.WhenAll(response, listType);

            if (response.Result.IsSuccessStatusCode && listType.Result.IsSuccessStatusCode)
            {
                string result = response.Result.Content.ReadAsStringAsync().Result;
                VehicleViewModel tmpModel = JsonConvert.DeserializeObject<VehicleViewModel>(result);

                string resultType = listType.Result.Content.ReadAsStringAsync().Result;
                List<TypeViewModel> tmpType = JsonConvert.DeserializeObject<List<TypeViewModel>>(resultType);

                ViewData["vehicleType"] = new SelectList(tmpType, "typeCode", "typeName");

                tmpModel.typeName = tmpType.Where(z => z.typeCode == tmpModel.typeCode).Select(x => x.typeName).FirstOrDefault();
                model = tmpModel;
            }
            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleViewModel vehicle)
        {
            HttpResponseMessage response = await this._repo.UpdateVehicle(vehicle);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Vehicle");

            TempData["Error"] = "Gagal insert data";

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

        
    }
}