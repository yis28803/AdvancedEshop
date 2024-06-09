using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;

namespace Duanmau.Web.Controllers
{

    public class Admin_KhachHangController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_KhachHangController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> KhachHangAll()
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

            HttpResponseMessage response = await client.GetAsync("api/KhachHang");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<KhachHang> KhachHang = JsonConvert.DeserializeObject<List<KhachHang>>(responseContent);

                return View("KhachHangAll", KhachHang);
                // Bây giờ bạn có thể sử dụng danh sách `foods` như bạn muốn
                // Ví dụ: Truyền danh sách foods đến view hoặc xử lý chúng theo cách khác
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }

        }
        public IActionResult KhachHangCreate()
        {
            return View("KhachHangCreate");
        }
        public async Task<IActionResult> KhachHangSaveAsync(KhachHang KhachHangs)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (KhachHangs.Name == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("KhachHangCreate", KhachHangs); // Trả về view với dữ liệu đã nhập
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
            string jsonKhachHangs = JsonConvert.SerializeObject(KhachHangs);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonKhachHangs, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để lưu sản phẩm
            HttpResponseMessage response = await client.PostAsync("api/KhachHang", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi lưu sản phẩm thành công
                return RedirectToAction("KhachHangAll");
            }
            else
            {
                // Xử lý khi lưu sản phẩm không thành công
                return View("KhachHangCreate");
            }
        }
        public async Task<IActionResult> KhachHangEdit(int id)
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

            HttpResponseMessage response = await client.GetAsync($"api/KhachHang/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                KhachHang KhachHangs = JsonConvert.DeserializeObject<KhachHang>(responseContent);

                return View("KhachHangEdit", KhachHangs);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> KhachHangSaveEditedAsync(KhachHang editedKhachHang)
        {
            // Kiểm tra xem các trường dữ liệu có giá trị null không
            if (editedKhachHang.Name == null)
            {
                // Nếu một trong các trường là null, hiển thị thông báo lỗi và không thực hiện yêu cầu
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                return View("KhachHangEdit", editedKhachHang); // Trả về view với dữ liệu đã nhập
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
            string jsonEditedKhachHang = JsonConvert.SerializeObject(editedKhachHang);

            // Tạo nội dung yêu cầu từ dữ liệu JSON
            var content = new StringContent(jsonEditedKhachHang, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến API để cập nhật sản phẩm
            HttpResponseMessage response = await client.PutAsync($"api/KhachHang/{editedKhachHang.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi cập nhật sản phẩm thành công
                return RedirectToAction("KhachHangAll");
            }
            else
            {
                // Xử lý khi cập nhật sản phẩm không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> KhachHangDeleteAsync(int id)
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
            HttpResponseMessage response = await client.DeleteAsync($"api/KhachHang/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Xử lý khi xóa sản phẩm thành công
                return RedirectToAction("KhachHangAll");
            }
            else
            {
                // Xử lý khi xóa sản phẩm không thành công
                return View("Error");
            }
        }
    }
}
