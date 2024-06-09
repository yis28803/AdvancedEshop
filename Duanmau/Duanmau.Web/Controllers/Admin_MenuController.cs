using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Duanmau.Web.Controllers
{
    public class Admin_MenuController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public Admin_MenuController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        /*public async Task<IActionResult> FoodAll()
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
        }*/
        public async Task<ActionResult> IndexAsync()
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
                return View("Index", foods);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
