using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Duanmau.Web.Controllers
{
    public class Admin_ThongKeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Admin_ThongKeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> RevenueStatistics()
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

            HttpResponseMessage response = await client.GetAsync("api/Bill/RevenueStatistics");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<RevenueStatistics> revenueStatistics = JsonConvert.DeserializeObject<List<RevenueStatistics>>(responseContent);

                return View(revenueStatistics);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }

        public async Task<IActionResult> ProductRevenueStatistics()
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

            HttpResponseMessage response = await client.GetAsync("api/BillInfo/ProductRevenueStatistics");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<ProductStatistics2> productStatistics = JsonConvert.DeserializeObject<List<ProductStatistics2>>(responseContent);

                return View(productStatistics);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }
        public async Task<IActionResult> ProductStatistics2()
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

            HttpResponseMessage response = await client.GetAsync("api/BillInfo/ProductRevenueStatistics2");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<ProductStatistics3> productStatistics = JsonConvert.DeserializeObject<List<ProductStatistics3>>(responseContent);

                return View(productStatistics);
            }
            else
            {
                // Xử lý trường hợp không thành công
                return View("Error");
            }
        }

    }
}
