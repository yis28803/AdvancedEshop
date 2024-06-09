using Microsoft.AspNetCore.Mvc;

namespace Duanmau.Web.Controllers
{
    public class TextController : Controller
    {
        public IActionResult Index()
        {
            // Lấy giá trị ID từ session
            var userId = HttpContext.Session.GetInt32("userId");

            // Truyền giá trị ID sang index.cshtml
            return View(userId);
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
    }
}
