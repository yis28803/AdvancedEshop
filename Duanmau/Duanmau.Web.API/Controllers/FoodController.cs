using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*[Authorize]*/
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Food/ThuNgan
        [HttpGet("ThuNgan")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodThuNgans()
        {
            // Lấy danh sách thức ăn từ cơ sở dữ liệu
            var foods = await _context.Foods.Where(f => f.Status == 1).ToListAsync();

            // Duyệt qua từng thức ăn để lấy thông tin của danh mục thức ăn tương ứng và gán vào mỗi đối tượng Food
            foreach (var food in foods)
            {
                var danhMucFood = await _context.DanhMucFoods.FindAsync(food.DanhMucFoodId);
                if (danhMucFood != null)
                {
                    // Thêm thông tin của danh mục thức ăn vào đối tượng Food
                    food.DanhMucFoods = danhMucFood;
                }
            }

            return foods;
        }


        // GET: api/Food
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            // Lấy danh sách thức ăn từ cơ sở dữ liệu
            var foods = await _context.Foods.ToListAsync();

            // Duyệt qua từng thức ăn để lấy thông tin của danh mục thức ăn tương ứng và gán vào mỗi đối tượng Food
            foreach (var food in foods)
            {
                var danhMucFood = await _context.DanhMucFoods.FindAsync(food.DanhMucFoodId);
                if (danhMucFood != null)
                {
                    // Thêm thông tin của danh mục thức ăn vào đối tượng Food
                    food.DanhMucFoods = danhMucFood;
                }
            }

            return foods;
        }

        // GET: api/Food/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        // GET: api/Food/ByCategory/{danhMucFoodId}
        [HttpGet("ByCategory/{danhMucFoodId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodsByCategory(int danhMucFoodId)
        {
            // Lấy danh sách thức ăn theo danh mục
            var foods = await _context.Foods.Where(f => f.DanhMucFoodId == danhMucFoodId).ToListAsync();

            // Kiểm tra nếu danh sách trống
            if (foods == null || foods.Count == 0)
            {
                return NotFound("Không có sản phẩm nào thuộc danh mục này.");
            }

            return foods;
        }

        // POST: api/Food
        [HttpPost]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
            var existingFood = await _context.Foods.FirstOrDefaultAsync(f => f.FoodName == food.FoodName);
            if (existingFood != null)
            {
                return Conflict("Món ăn đã tồn tại");
            }

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFood), new { id = food.FoodId }, food);
        }

        // PUT: api/Food/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int id, Food food)
        {
            if (id != food.FoodId)
            {
                return BadRequest();
            }

            var existingFood = await _context.Foods.FirstOrDefaultAsync(f => f.FoodName == food.FoodName && f.FoodId != id);
            if (existingFood != null)
            {
                return Conflict("Món ăn đã tồn tại");
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Food/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.FoodId == id);
        }
    }
}
