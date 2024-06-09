using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duanmau.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DanhMucFoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DanhMucFoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DanhMucFood
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMucFood>>> GetDanhMucFoods()
        {
            return await _context.DanhMucFoods.ToListAsync();
        }

        // GET: api/DanhMucFood/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhMucFood>> GetDanhMucFood(int id)
        {
            var danhMucFood = await _context.DanhMucFoods.FindAsync(id);

            if (danhMucFood == null)
            {
                return NotFound();
            }

            return danhMucFood;
        }

        // POST: api/DanhMucFood
        [HttpPost]
        public async Task<ActionResult<DanhMucFood>> PostDanhMucFood(DanhMucFood danhMucFood)
        {
            var existingDanhMucFood = await _context.DanhMucFoods.FirstOrDefaultAsync(df => df.DanhMucFoodName == danhMucFood.DanhMucFoodName);
            if (existingDanhMucFood != null)
            {
                return Conflict("Danh mục thức ăn đã tồn tại");
            }

            _context.DanhMucFoods.Add(danhMucFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDanhMucFood), new { id = danhMucFood.DanhMucFoodId }, danhMucFood);
        }


        // PUT: api/DanhMucFood/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDanhMucFood(int id, DanhMucFood danhMucFood)
        {
            if (id != danhMucFood.DanhMucFoodId)
            {
                return BadRequest();
            }

            var existingDanhMucFood = await _context.DanhMucFoods.FirstOrDefaultAsync(df => df.DanhMucFoodName == danhMucFood.DanhMucFoodName && df.DanhMucFoodId != id);
            if (existingDanhMucFood != null)
            {
                return Conflict("Danh mục thức ăn đã tồn tại");
            }

            _context.Entry(danhMucFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DanhMucFoodExists(id))
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


        // DELETE: api/DanhMucFood/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhMucFood(int id)
        {
            var danhMucFood = await _context.DanhMucFoods.FindAsync(id);
            if (danhMucFood == null)
            {
                return NotFound();
            }

            _context.DanhMucFoods.Remove(danhMucFood);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DanhMucFoodExists(int id)
        {
            return _context.DanhMucFoods.Any(e => e.DanhMucFoodId == id);
        }
    }
}
