using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;

namespace Duanmau.Web.Controllers
{
    public class Admin_IngredientController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_IngredientController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> IngredientAll()
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

            HttpResponseMessage response = await client.GetAsync("api/Ingredient");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<Ingredient> Ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(responseContent);

                return View("IngredientAll", Ingredients);
                // Bây giờ bạn có thể sử dụng danh sách `foods` như bạn muốn
                // Ví dụ: Truyền danh sách foods đến view hoặc xử lý chúng theo cách khác
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

        }
        public IActionResult IngredientCreate()
        {
            return View("IngredientCreate");
        }
        public async Task<IActionResult> IngredientSaveAsync(Ingredient Ingredients)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (Ingredients.IngredientName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("IngredientCreate", Ingredients); // Trả về view với dữ liệu đã nhập
            }
            // Nếu các trường dữ liệu không null, tiếp tục gửi yêu cầu tới API để thêm mới
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            // Chuyển đối tượng food thành dữ liệu JSON
            string jsonIngredients = JsonConvert.SerializeObject(Ingredients);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonIngredients, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để lưu sản phẩm
            HttpResponseMessage response = await client.PostAsync("api/Ingredient", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi lưu sản phẩm thành công
                return RedirectToAction("IngredientAll");
            }
            else
            {
                // Xử lý khi lưu sản phẩm không thành công
                return View("IngredientCreate");
            }
        }
        public async Task<IActionResult> IngredientEdit(int id)
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

            HttpResponseMessage response = await client.GetAsync($"api/Ingredient/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Ingredient Ingredients = JsonConvert.DeserializeObject<Ingredient>(responseContent);

                return View("IngredientEdit", Ingredients);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> IngredientSaveEditedAsync(Ingredient editedIngredient)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (editedIngredient.IngredientName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("IngredientEdit", editedIngredient); // Trả về view với dữ liệu đã nhập
            }

            // Nếu các trường dữ liệu không null, tiếp tục gửi yêu cầu tới API để chỉnh sửa
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            // Chuyển đối tượng food chỉnh sửa thành dữ liệu JSON
            string jsonEditedIngredient = JsonConvert.SerializeObject(editedIngredient);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonEditedIngredient, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để cập nhật sản phẩm
            HttpResponseMessage response = await client.PutAsync($"api/Ingredient/{editedIngredient.IngredientId}", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi cập nhật sản phẩm thành công
                return RedirectToAction("IngredientAll");
            }
            else
            {
                // Xử lý khi cập nhật sản phẩm không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> IngredientDeleteAsync(int id)
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

            // Gửi yêu cầu DELETE đến API để xóa sản phẩm
            HttpResponseMessage response = await client.DeleteAsync($"api/Ingredient/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi xóa sản phẩm thành công
                return RedirectToAction("IngredientAll");
            }
            else
            {
                // Xử lý khi xóa sản phẩm không thành công
                return View("Error");
            }
        }
    }
}
