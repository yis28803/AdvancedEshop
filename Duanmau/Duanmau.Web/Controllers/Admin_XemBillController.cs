using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Duanmau.Web.Controllers
{
    public class Admin_XemBillController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_XemBillController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> GetBills()
        {
            try
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

                HttpResponseMessage response = await client.GetAsync("api/Bill/xuatbill");

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<BillViewModels3> bills = JsonConvert.DeserializeObject<List<BillViewModels3>>(responseContent);

                    // Sắp xếp danh sách hóa đơn theo mã hóa đơn từ lớn đến nhỏ
                    bills = bills.OrderByDescending(b => b.BillId).ToList();

                    return View(bills);
                }
                else
                {
                    // Xử lý trường hợp không thành công
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBill(int id)
        {
            // Gọi API để xóa hóa đơn từ Controller BillController
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            var response = await client.DeleteAsync($"api/Bill/{id}");*/
            var token = HttpContext.Session.GetString("token");
            if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.DeleteAsync($"api/Bill/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Chuyển hướng người dùng về trang GetBills
                return RedirectToAction(nameof(GetBills));
            }
            else
            {
                // Xử lý khi xóa không thành công (nếu cần)
                return StatusCode((int)response.StatusCode);
            }
        }

    }
}
