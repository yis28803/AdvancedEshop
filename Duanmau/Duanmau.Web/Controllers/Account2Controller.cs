using Duanmau.Web.API.Models;
using Duanmau.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Duanmau.Web.Controllers
{
    public class Account2Controller : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public Account2Controller(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri("https://localhost:7152/");

                var requestContent = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Account/login", requestContent);

                if (response.IsSuccessStatusCode)
                {

                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Chuyển đổi chuỗi JSON thành đối tượng JObject
                    var jsonObject = JObject.Parse(responseContent);

                    // Lấy giá trị token từ đối tượng JObject
                    var token = jsonObject["token"].ToString();

                    // Lưu token vào session
                    HttpContext.Session.SetString("token", token);


                    var token1 = await response.Content.ReadAsStringAsync();

                    var principal = DecodeJwtToken(token1);

                    var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                    {
                        HttpContext.Session.SetInt32("userId", userId);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác");
                }
            }

            return View(loginModel);
        }
        public ClaimsPrincipal DecodeJwtToken(string tokenData)
        {
            // Chuyển dữ liệu JSON thành đối tượng token
            var jsonObject = JObject.Parse(tokenData);
            var token = jsonObject["token"].ToString();

            var tokenHandler = new JwtSecurityTokenHandler();

            // Kiểm tra xem token có đúng định dạng không
            if (!tokenHandler.CanReadToken(token))
            {
                throw new SecurityTokenException("Invalid token format");
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsTheSecureKey12345678901234567890")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal principal = null;
            try
            {
                principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            }
            catch (SecurityTokenException ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }

            return principal;
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa token khỏi session
            HttpContext.Session.Remove("token");

            return RedirectToAction("Login");
        }
    }
}
