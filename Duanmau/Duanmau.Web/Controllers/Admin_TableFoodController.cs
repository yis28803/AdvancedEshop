using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Duanmau.Web.Controllers
{
    public class Admin_TableFoodController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_TableFoodController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> TableFoodAll()
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

            HttpResponseMessage response = await client.GetAsync("api/TableFood");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<Tablefood> tablefoods = JsonConvert.DeserializeObject<List<Tablefood>>(responseContent);

                return View("TableFoodAll", tablefoods);
                // Bây giờ bạn có thể sử dụng danh sách `foods` như bạn muốn
                // Ví dụ: Truyền danh sách foods đến view hoặc xử lý chúng theo cách khác
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

        }
        public IActionResult TableFoodCreate()
        {
            return View("TableFoodCreate");
        }
        public async Task<IActionResult> TableFoodSaveAsync(Tablefood Tablefoods)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (Tablefoods.TablefoodName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("TableFoodCreate", Tablefoods); // Trả về view với dữ liệu đã nhập
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
            string jsonTableFoods = JsonConvert.SerializeObject(Tablefoods);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonTableFoods, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để lưu sản phẩm
            HttpResponseMessage response = await client.PostAsync("api/TableFood", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi lưu sản phẩm thành công
                return RedirectToAction("TableFoodAll");
            }
            else
            {
                // Xử lý khi lưu sản phẩm không thành công
                return View("TableFoodCreate");
            }
        }
        public async Task<IActionResult> TableFoodEdit(int id)
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

            HttpResponseMessage response = await client.GetAsync($"api/TableFood/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Tablefood TableFoods = JsonConvert.DeserializeObject<Tablefood>(responseContent);

                return View("TableFoodEdit", TableFoods);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> TableFoodSaveEditedAsync(Tablefood editedTableFood)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (editedTableFood.TablefoodName == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("TableFoodEdit", editedTableFood); // Trả về view với dữ liệu đã nhập
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
            string jsonEditedTableFood = JsonConvert.SerializeObject(editedTableFood);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonEditedTableFood, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để cập nhật sản phẩm
            HttpResponseMessage response = await client.PutAsync($"api/TableFood/{editedTableFood.TablefoodId}", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi cập nhật sản phẩm thành công
                return RedirectToAction("TableFoodAll");
            }
            else
            {
                // Xử lý khi cập nhật sản phẩm không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> TableFoodDeleteAsync(int id)
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
            HttpResponseMessage response = await client.DeleteAsync($"api/TableFood/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi xóa sản phẩm thành công
                return RedirectToAction("TableFoodAll");
            }
            else
            {
                // Xử lý khi xóa sản phẩm không thành công
                return View("Error");
            }
        }
    }
}
