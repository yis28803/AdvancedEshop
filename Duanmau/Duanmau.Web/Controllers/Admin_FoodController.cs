using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace Duanmau.Web.Controllers
{
    public class Admin_FoodController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _env;
        public Admin_FoodController(IHttpClientFactory clientFactory, IWebHostEnvironment env)
        {
            _clientFactory = clientFactory;
            _env = env;
        }
        public async Task<IActionResult> FoodAll()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync("api/Food");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<Food> foods = JsonConvert.DeserializeObject<List<Food>>(responseContent);
                return View("FoodAll", foods);
            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> FoodCreate()
        {
            List<DanhMucFood> danhMucFoodsList = await GetDanhMucFoodsList();
            ViewBag.DanhMucFoodsList = danhMucFoodsList;
            return View("FoodCreate");
        }
        private async Task<List<DanhMucFood>> GetDanhMucFoodsList()
        {
            var token = HttpContext.Session.GetString("token");

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync("api/DanhMucFood");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<DanhMucFood> danhMucFoods = JsonConvert.DeserializeObject<List<DanhMucFood>>(responseContent);
                return danhMucFoods;
            }
            else
            {
                return new List<DanhMucFood>();
            }
        }

        public async Task<IActionResult> FoodSaveAsync(Food food, IFormFile file)
        {
            if (food.FoodName == null || file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin và chọn hình ảnh.");
                return View("FoodCreate", food);
            }
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var imagePath = await SaveImageAsync(file);
            food.File = imagePath;
            string jsonFood = JsonConvert.SerializeObject(food);
            var content = new StringContent(jsonFood, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/Food", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FoodAll");
            }
            else
            {
                return View("FoodCreate");
            }
        }
        public async Task<IActionResult> FoodEdit(int id)
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage responseFood = await client.GetAsync($"api/Food/{id}");
            HttpResponseMessage responseDanhMuc = await client.GetAsync("api/DanhMucFood");
            if (responseFood.IsSuccessStatusCode && responseDanhMuc.IsSuccessStatusCode)
            {
                string responseContentFood = await responseFood.Content.ReadAsStringAsync();
                Food food = JsonConvert.DeserializeObject<Food>(responseContentFood);
                string responseContentDanhMuc = await responseDanhMuc.Content.ReadAsStringAsync();
                List<DanhMucFood> danhMucFoods = JsonConvert.DeserializeObject<List<DanhMucFood>>(responseContentDanhMuc);
                DanhMucFood danhMucFoodOfFood = danhMucFoods.FirstOrDefault(d => d.DanhMucFoodId == food.DanhMucFoodId);
                ViewBag.DanhMucFoodsList = danhMucFoods;
                ViewBag.CurrentDanhMucFoodId = danhMucFoodOfFood.DanhMucFoodId;

                return View("FoodEdit", food);
            }
            else
            {
                ViewBag.DanhMucFoodsList = new List<DanhMucFood>(); // Gán một danh sách rỗng
                return View("Error");
            }
        }
        public async Task<IActionResult> FoodSaveEditedAsync(Food editedFood, IFormFile file)
        {
            if (editedFood.FoodName == null || file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin và chọn hình ảnh.");
                return View("Error"); 
            }
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var imagePath = await SaveImageAsync(file);
            editedFood.File = imagePath;
            string jsonEditedFood = JsonConvert.SerializeObject(editedFood);
            var content = new StringContent(jsonEditedFood, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"api/Food/{editedFood.FoodId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FoodAll");
            }
            else
            {
                return View("Error");
            }
        }
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}-{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return "/uploads/" + fileName;
        }
        public async Task<IActionResult> FoodDeleteAsync(int id)
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            /* var client = _clientFactory.CreateClient();
             client.BaseAddress = new Uri("https://localhost:7152/");
             client.DefaultRequestHeaders.Accept.Clear();
             client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            HttpResponseMessage response = await client.DeleteAsync($"api/Food/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("FoodAll");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
