using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Duanmau.Web.Controllers
{
    public class Admin_DanhMucFoodController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_DanhMucFoodController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> DanhMucFoodAll()
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

            HttpResponseMessage response = await client.GetAsync("api/DanhMucFood");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<DanhMucFood> danhMucFoods = JsonConvert.DeserializeObject<List<DanhMucFood>>(responseContent);

                return View("DanhMucFoodAll", danhMucFoods);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }

        public IActionResult DanhMucFoodCreate()
        {
            return View("DanhMucFoodCreate");
        }

        [HttpPost]
        public async Task<IActionResult> DanhMucFoodSaveAsync(DanhMucFood danhMucFood)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (danhMucFood.DanhMucFoodName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("DanhMucFoodCreate", danhMucFood); // Trả về view với dữ liệu đã nhập
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

            string jsonDanhMucFood = JsonConvert.SerializeObject(danhMucFood);

            var content = new StringContent(jsonDanhMucFood, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/DanhMucFood", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DanhMucFoodAll");
            }
            else
            {
                return View("DanhMucFoodCreate");
            }
        }

        public async Task<IActionResult> DanhMucFoodEdit(int id)
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

            HttpResponseMessage response = await client.GetAsync($"api/DanhMucFood/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                DanhMucFood danhMucFood = JsonConvert.DeserializeObject<DanhMucFood>(responseContent);

                return View("DanhMucFoodEdit", danhMucFood);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DanhMucFoodSaveEditedAsync(DanhMucFood editedDanhMucFood)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (editedDanhMucFood.DanhMucFoodName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("DanhMucFoodEdit", editedDanhMucFood); // Trả về view với dữ liệu đã nhập
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

            string jsonEditedDanhMucFood = JsonConvert.SerializeObject(editedDanhMucFood);

            var content = new StringContent(jsonEditedDanhMucFood, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync($"api/DanhMucFood/{editedDanhMucFood.DanhMucFoodId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DanhMucFoodAll");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DanhMucFoodDeleteAsync(int id)
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

            HttpResponseMessage response = await client.DeleteAsync($"api/DanhMucFood/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DanhMucFoodAll");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
