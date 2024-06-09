using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Duanmau.Web.Controllers
{
    public class Admin_NhanVienController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_NhanVienController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> NhanVienAll()
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

            HttpResponseMessage response = await client.GetAsync("api/NhanVien");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<NhanVien> NhanVien = JsonConvert.DeserializeObject<List<NhanVien>>(responseContent);

                return View("NhanVienAll", NhanVien);
                // Bây giờ bạn có thể sử dụng danh sách `foods` như bạn muốn
                // Ví dụ: Truyền danh sách foods đến view hoặc xử lý chúng theo cách khác
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

        }
        public IActionResult NhanVienCreate()
        {
            return View("NhanVienCreate");
        }
        public async Task<IActionResult> NhanVienSaveAsync(NhanVien NhanViens)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (NhanViens.Name == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("NhanVienCreate", NhanViens); // Trả về view với dữ liệu đã nhập
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
            string jsonNhanViens = JsonConvert.SerializeObject(NhanViens);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonNhanViens, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để lưu sản phẩm
            HttpResponseMessage response = await client.PostAsync("api/NhanVien", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi lưu sản phẩm thành công
                return RedirectToAction("NhanVienAll");
            }
            else
            {
                // Xử lý khi lưu sản phẩm không thành công
                return View("NhanVienCreate");
            }
        }
        public async Task<IActionResult> NhanVienEdit(int id)
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

            HttpResponseMessage response = await client.GetAsync($"api/NhanVien/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                NhanVien NhanViens = JsonConvert.DeserializeObject<NhanVien>(responseContent);

                return View("NhanVienEdit", NhanViens);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> NhanVienSaveEditedAsync(NhanVien editedNhanVien)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (editedNhanVien.Name == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("NhanVienEdit", editedNhanVien); // Trả về view với dữ liệu đã nhập
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
            string jsonEditedNhanVien = JsonConvert.SerializeObject(editedNhanVien);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonEditedNhanVien, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để cập nhật sản phẩm
            HttpResponseMessage response = await client.PutAsync($"api/NhanVien/{editedNhanVien.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi cập nhật sản phẩm thành công
                return RedirectToAction("NhanVienAll");
            }
            else
            {
                // Xử lý khi cập nhật sản phẩm không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> NhanVienDeleteAsync(int id)
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
            HttpResponseMessage response = await client.DeleteAsync($"api/NhanVien/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi xóa sản phẩm thành công
                return RedirectToAction("NhanVienAll");
            }
            else
            {
                // Xử lý khi xóa sản phẩm không thành công
                return View("Error");
            }
        }
    }
}
