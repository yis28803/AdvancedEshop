using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Duanmau.Web.Models;
using Duanmau.Web.API.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Duanmau.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var foods = await GetFoodsAsync();
            var danhMucFoods = await GetDanhMucFoodsAsync();
            // Lấy giá trị ID từ session
            var userId = HttpContext.Session.GetInt32("userId");

            if (foods == null || danhMucFoods == null)
            {
                return View("Error");
            }
            // Trả về view với model item
            return View((userId, danhMucFoods));
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userId");

            HttpContext.Session.Remove("token");

            return RedirectToAction("Index");
        }

        public IActionResult SetUserId(int userId)
        {
            // Lưu giá trị ID vào session khi người dùng đăng nhập thành công
            HttpContext.Session.SetInt32("userId", userId);
            return RedirectToAction("Index");
        }

        public IActionResult ClearUserId()
        {
            // Xóa giá trị ID khỏi session khi người dùng đăng xuất
            HttpContext.Session.Remove("userId");
            return RedirectToAction("Index");
        }

        private async Task<List<Tablefood>> GetTableFoodsAsync()
        {
            return await GetDataAsync<List<Tablefood>>("api/Tablefood");
        }

        private async Task<List<Food>> GetFoodsAsync()
        {
            return await GetDataAsync<List<Food>>("api/Food/ThuNgan");
        }

        private async Task<List<DanhMucFood>> GetDanhMucFoodsAsync()
        {
            return await GetDataAsync<List<DanhMucFood>>("api/DanhMucFood");
        }

        private async Task<T> GetDataAsync<T>(string apiEndpoint)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/

            /*if (token == null)
            {
                return RedirectToAction("Login", "Account2");
            }*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync(apiEndpoint);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        private async Task<List<Food>> GetFoodsByDanhMucFoodIdAsync(int danhMucFoodId)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync($"api/Food/ByCategory/{danhMucFoodId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Food>>();
            }
            else
            {
                return null;
            }
        }
        private async Task<List<Tablefood>> GetTableFoodsAsync(bool? status)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.GetAsync($"api/Tablefood/loc?status={status}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Tablefood>>();
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await GetFoodsAsync();
            if (foods == null)
            {
                return NotFound("Không có sản phẩm nào.");
            }
            return PartialView("_FoodsPartial", foods);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsByCategory(int danhMucFoodId)
        {
            var foods = await GetFoodsByDanhMucFoodIdAsync(danhMucFoodId);
            if (foods == null)
            {
                return NotFound("Không có sản phẩm nào thuộc danh mục này.");
            }
            return PartialView("_FoodsPartial", foods);
            
        }

        [HttpGet]
        public async Task<IActionResult> GetTableFoodsByStatus(bool? status)
        {
            var tableFoods = await GetTableFoodsAsync(status);
            if (tableFoods == null)
            {
                return NotFound("Không có phòng/bàn nào.");
            }
            return PartialView("_TableFoodsPartial", tableFoods);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllTableFoods()
        {
            var tableFoods = await GetTableFoodsAsync();
            if (tableFoods == null)
            {
                return NotFound("Không có phòng/bàn nào.");
            }
            return PartialView("_TableFoodsPartial", tableFoods);
        }

        [HttpPost]
        public async Task<IActionResult> LockTable(string selectedTablefoodId)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.PostAsync($"api/Tablefood/LockTable?selectedTablefoodId={selectedTablefoodId}", null);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return Ok(message);
            }
            else
            {
                return BadRequest("Đã xảy ra lỗi khi khóa bàn.");
            }
        }
        [HttpPost]
        public async Task<IActionResult> OpenTable(string selectedTablefoodId)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await client.PostAsync($"api/Tablefood/OpenTable?selectedTablefoodId={selectedTablefoodId}", null);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return Ok(message);
            }
            else
            {
                return BadRequest("Đã xảy ra lỗi khi khóa bàn.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CheckoutPartialAsync(string tableData, string idtablefood)
        {
            var extractedData = new List<CheckoutItem>();

            // Lấy userId từ Session
            var userId = HttpContext.Session.GetInt32("userId");
            // Lấy danh sách danh mục thức ăn từ API hoặc cơ sở dữ liệu
            List<KhachHang> KhachHangsList = await GetKhachHangsList();

            if (!string.IsNullOrEmpty(tableData))
            {
                var dataDict = JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(tableData);

                foreach (var kvp in dataDict)
                {
                    if (kvp.Key == int.Parse(idtablefood))
                    {
                        var key = kvp.Key;
                        var foodTable = kvp.Value.foodTable.ToString();
                        var totalValueString = kvp.Value.totalValue.ToString();
                        var totalValueDecimal = ExtractDecimalValue(totalValueString);

                        var foodItems = ExtractFoodItems(foodTable);

                        extractedData.Add(new CheckoutItem { Key = key, FoodItems = foodItems, TotalValue = totalValueDecimal });

                        break;
                    }
                }
            }

            // Đặt userId vào ViewBag để sử dụng trong _CheckoutPartial.cshtml
            ViewBag.UserId = userId;
            // Truyền danh sách danh mục thức ăn vào View
            ViewBag.KhachHangsList = KhachHangsList;


            return PartialView("_CheckoutPartial", extractedData);
        }


        private async Task<List<KhachHang>> GetKhachHangsList()
        {
            // Thực hiện lấy danh sách danh mục thức ăn từ API hoặc cơ sở dữ liệu
            // Ví dụ:
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await client.GetAsync("api/KhachHang");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                List<KhachHang> KhachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(responseContent);
                return KhachHangs;
            }
            else
            {
                // Xử lý trường hợp không thành công
                return new List<KhachHang>();
            }
        }
        private List<FoodItem> ExtractFoodItems(string foodTableHtml)
        {
            var foodItems = new List<FoodItem>();
            var regex = new Regex(@"<tr><td>(.*?)</td><td>(\d+)</td><td>(\d+\.\d+)</td><td>(\d+\.\d+)</td><td><button.*?value=""(\d+)"".*?</tr>");
            var matches = regex.Matches(foodTableHtml);
            foreach (Match match in matches)
            {
                var name = match.Groups[1].Value;
                var quantity = int.Parse(match.Groups[2].Value);
                var price = decimal.Parse(match.Groups[3].Value);
                var totalPrice = decimal.Parse(match.Groups[4].Value);
                var idFood = int.Parse(match.Groups[5].Value);
                var foodItem = new FoodItem { Name = name, Quantity = quantity, Price = price, TotalPrice = totalPrice, IDFood = idFood };
                foodItems.Add(foodItem);
            }
            return foodItems;
        }
        private decimal? ExtractDecimalValue(string totalValue)
        {
            var regex = new Regex(@"\d+\.\d+");
            var match = regex.Match(totalValue);
            if (match.Success)
            {
                var valueString = match.Value;
                if (decimal.TryParse(valueString, out decimal result))
                {
                    return result;
                }
            }
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBillAndBillInfo(DateTime dateCheckIn, DateTime dateCheckOut, int idNhanVien, int idKhachHang, int idTableFood, decimal totalValue, Dictionary<string, BillInfoViewModel> billInfos)
        {
            var billViewModel = new BillViewModels
            {
                DateCheckIn = dateCheckIn,
                DateCheckOut = dateCheckOut,
                IdNhanVien = idNhanVien,
                IdKhachHang = idKhachHang,
                IdTableFood = idTableFood,
                TotalValue = totalValue
            };

            var createBillResponse = await CreateBillAsync(billViewModel);

            if (!createBillResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var billResponseContent = await createBillResponse.Content.ReadAsStringAsync();
            var createdBill = JsonConvert.DeserializeObject<Bill>(billResponseContent);

            var billInfoViewModels = new List<BillInfoViewModels>();

            foreach (var entry in billInfos)
            {
                var checkoutItemId = entry.Key;
                var billInfoViewModel = entry.Value;

                var newBillInfo = new BillInfoViewModels
                {
                    IdBill = createdBill.BillId,
                    IdFood = billInfoViewModel.IdFood,
                    Count = billInfoViewModel.Count,
                    Price = billInfoViewModel.Price,
                    TotalPrice = billInfoViewModel.TotalPrice,
                    GiamGia = billInfoViewModel.GiamGia
                };

                billInfoViewModels.Add(newBillInfo);
            }

            var createBillInfoResponses = await CreateBillInfosAsync(createdBill.BillId, billInfoViewModels);

            if (!createBillInfoResponses.All(response => response.IsSuccessStatusCode))
            {
                return View("Error");
            }

            /*return RedirectToAction("Success");*/
            TempData["SuccessMessage"] = "Thêm bill thành công";

            return RedirectToAction("Index");
        }

        private async Task<HttpResponseMessage> CreateBillAsync(BillViewModels billViewModel)
        {
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var jsonBillViewModel = JsonConvert.SerializeObject(billViewModel);
            var content = new StringContent(jsonBillViewModel, Encoding.UTF8, "application/json");

            return await client.PostAsync("api/Bill", content);
        }

        private async Task<List<HttpResponseMessage>> CreateBillInfosAsync(int billId, List<BillInfoViewModels> billInfoViewModels)
        {
            var responses = new List<HttpResponseMessage>();

            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            foreach (var billInfoViewModel in billInfoViewModels)
            {
                billInfoViewModel.IdBill = billId; // Set ID của hóa đơn cho mỗi thông tin hóa đơn
                var jsonBillInfoViewModel = JsonConvert.SerializeObject(billInfoViewModel);
                var content = new StringContent(jsonBillInfoViewModel, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/BillInfo", content);
                responses.Add(response);
            }

            return responses;
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
            /*var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7152/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
            var token = HttpContext.Session.GetString("token");
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
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý khi lưu sản phẩm không thành công
                return View("KhachHangCreate");
            }
        }





    }
}
