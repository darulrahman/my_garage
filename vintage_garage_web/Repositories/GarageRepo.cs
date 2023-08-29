using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using vintage_garage_web.Models;
using vintage_garage_web.Models.Login;

namespace vintage_garage_web.Repositories
{
    public class GarageRepo : iGarageRepo
    {
        private readonly IConfiguration _Configuration;
        private readonly string baseURL = "";
        private string jwtToken = "";
        public GarageRepo(IConfiguration configuration)
        {
            _Configuration = configuration;
            baseURL = _Configuration.GetSection("BaseURL").GetSection("gatewayAPI").Value;
        }
        
        public async Task<HttpResponseMessage> GetAllVehicles()
        {
            HttpResponseMessage getData;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.GetAsync("vehicle");
            }

            return getData;
        }

        public async Task<HttpResponseMessage> AddVehicle(VehicleReq vehicle)
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.PostAsync("vehicle", content);
            }

            return getData;
        }

        public async Task<HttpResponseMessage> UpdateVehicle(VehicleReq vehicle)
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.PutAsync("vehicle", content);
            }

            return getData;
        }

       

        public async Task<HttpResponseMessage> DeleteVehicle(int id)
        {
            HttpResponseMessage getData;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.DeleteAsync("vehicle/" + id.ToString());
            }

            return getData;
        }

        public async Task<HttpResponseMessage> GetVehiclesById(int id)
        {
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                result = await client.GetAsync("vehicle/" + id.ToString());
            }

            return result;
        }

        public async Task<HttpResponseMessage> GetAllType()
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.GetAsync("type");
            }

            return getData;
        }

        public async Task<HttpResponseMessage> GetType(string code)
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.GetAsync("type/" + code);
            }

            return getData;
        }

        public async Task<HttpResponseMessage> Login(LoginReq log)
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(log), Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.PostAsync("signin", content);
            }

            return getData;
        }

        public string GetVehicleImage(string vehicleType)
        {
            string strUrl = "";
            switch (vehicleType)
            {
                case "bk":
                    strUrl = "https://cdn-icons-png.flaticon.com/512/568/568148.png";
                    break;
                case "car":
                    strUrl = "https://cdn-icons-png.flaticon.com/512/3774/3774270.png";
                    break;
                case "trn":
                    strUrl = "https://www.freeiconspng.com/uploads/transportation-icon-png-3.png";
                    break;
                case "bus":
                    strUrl = "https://cdn-icons-png.flaticon.com/512/272/272861.png";
                    break;
                case "trk":
                    strUrl = "https://cdn-icons-png.flaticon.com/512/215/215856.png";
                    break;
                default:
                    strUrl = "https://cdn-icons-png.flaticon.com/512/190/190190.png";
                    break;
            }

            return strUrl;
        }

        public async Task<HttpResponseMessage> GetAllCategories()
        {
            HttpResponseMessage getData;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                getData = await client.GetAsync("Category");
            }

            return getData;
        }
    }
}
