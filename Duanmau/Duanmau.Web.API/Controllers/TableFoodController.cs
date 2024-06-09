using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duanmau.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TableFoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TableFoodController(ApplicationDbContext context)
        {
            _context = context;
        }


        // POST: api/TableFood/LockTable
        [HttpPost("LockTable")]
        public async Task<IActionResult> LockTable(string selectedTablefoodId)
        {
            // Tìm bàn trong cơ sở dữ liệu
            var tableFood = await _context.Tablefoods.FindAsync(int.Parse(selectedTablefoodId));

            // Nếu bàn không tồn tại, trả về NotFound
            if (tableFood == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái của bàn thành "đã khóa"
            tableFood.Status = true;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return Ok("Bàn đã được khóa thành công!");
            }
            catch (Exception)
            {
                // Nếu có lỗi xảy ra, trả về BadRequest
                return BadRequest("Đã xảy ra lỗi khi khóa bàn!");
            }
        }
        // POST: api/TableFood/OpenTable
        [HttpPost("OpenTable")]
        public async Task<IActionResult> OpenTable(string selectedTablefoodId)
        {
            // Tìm bàn trong cơ sở dữ liệu
            var tableFood = await _context.Tablefoods.FindAsync(int.Parse(selectedTablefoodId));

            // Nếu bàn không tồn tại, trả về NotFound
            if (tableFood == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái của bàn thành "đã mở"
            tableFood.Status = false;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return Ok("Bàn đã được mở thành công!");
            }
            catch (Exception)
            {
                // Nếu có lỗi xảy ra, trả về BadRequest
                return BadRequest("Đã xảy ra lỗi khi mở bàn!");
            }
        }
        // GET: api/TableFood
        [HttpGet("loc")]
        public async Task<ActionResult<IEnumerable<Tablefood>>> GetTableFoods(bool? status)
        {
            IQueryable<Tablefood> query = _context.Tablefoods;

            // Lọc theo trạng thái nếu có
            if (status != null)
            {
                query = query.Where(tf => tf.Status == status);
            }

            return await query.ToListAsync();
        }

        // GET: api/TableFood
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tablefood>>> GetTableFoods()
        {
            return await _context.Tablefoods.ToListAsync();
        }

        // GET: api/TableFood/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tablefood>> GetTableFood(int id)
        {
            var tableFood = await _context.Tablefoods.FindAsync(id);

            if (tableFood == null)
            {
                return NotFound();
            }

            return tableFood;
        }

        // POST: api/TableFood
        [HttpPost]
        public async Task<ActionResult<Tablefood>> PostTableFood(Tablefood tableFood)
        {
            // Kiểm tra xem đã tồn tại bản ghi có cùng TableFoodName chưa
            var existingTableFood = await _context.Tablefoods.FirstOrDefaultAsync(tf => tf.TablefoodName == tableFood.TablefoodName);
            if (existingTableFood != null)
            {
                return Conflict("Tên bàn đã tồn tại.");
            }
            _context.Tablefoods.Add(tableFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTableFood), new { id = tableFood.TablefoodId }, tableFood);
        }

        // PUT: api/TableFood/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTableFood(int id, Tablefood tableFood)
        {
            if (id != tableFood.TablefoodId)
            {
                return BadRequest();
            }
            // Kiểm tra xem đã tồn tại bản ghi khác có cùng TableFoodName không (ngoại trừ bản ghi được cập nhật)
            var existingTableFood = await _context.Tablefoods.FirstOrDefaultAsync(tf => tf.TablefoodName == tableFood.TablefoodName && tf.TablefoodId != id);
            if (existingTableFood != null)
            {
                return Conflict("Tên bàn đã tồn tại.");
            }

            _context.Entry(tableFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableFoodExists(id))
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

        // DELETE: api/TableFood/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableFood(int id)
        {
            var tableFood = await _context.Tablefoods.FindAsync(id);
            if (tableFood == null)
            {
                return NotFound();
            }

            _context.Tablefoods.Remove(tableFood);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TableFoodExists(int id)
        {
            return _context.Tablefoods.Any(e => e.TablefoodId == id);
        }
    }
}
