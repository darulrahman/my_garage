using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Collections.Generic;
using vintage_garage_web.Models;
using vintage_garage_web.Models.Login;
using vintage_garage_web.Repositories;

namespace vintage_garage_web.Controllers
{
    public class LoginController : Controller
    {
        private iGarageRepo _repo;
        public LoginController(iGarageRepo repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            var strToken = HttpContext.Session.GetString("JwToken");
            if(!string.IsNullOrWhiteSpace(strToken))
                return RedirectToAction("Index", "Vehicle");

            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginReq log)
        {
            HttpResponseMessage logRes = new HttpResponseMessage();
            logRes = await _repo.Login(log);

            if(logRes.IsSuccessStatusCode)
            {
                string strLogRs = logRes.Content.ReadAsStringAsync().Result;
                LoginRes logRs = JsonConvert.DeserializeObject<LoginRes>(strLogRs);
                
                HttpContext.Session.SetString("JwToken", logRs.AccessToken);

                return RedirectToAction("Index", "Vehicle");
            }

            return View("Index");
        }
    }
}
