using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duanmau.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillInfoController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/BillInfo/ProductRevenueStatistics
        [HttpGet("ProductRevenueStatistics")]
        public async Task<ActionResult<IEnumerable<ProductStatistics2>>> GetProductRevenueStatistics()
        {
            var productRevenueStatistics = await _context.BillInfos
                .Where(bi => bi.Bills.DateCheckOut.Month == DateTime.Now.Month)
                .GroupBy(bi => bi.IdFood)
                .Select(g => new ProductStatistics2
                {
                    ProductId = g.Key,
                    ProductName = g.First().Foods.FoodName,
                    TotalRevenue = g.Sum(bi => bi.TotalPrice),
                    TotalQuantitySold = g.Sum(bi => bi.Count)
                })
                .ToListAsync();

            return productRevenueStatistics;
        }
        // GET: api/BillInfo/ProductRevenueStatistics2
        [HttpGet("ProductRevenueStatistics2")]
        public async Task<ActionResult<IEnumerable<ProductStatistics3>>> GetProductRevenueStatistics2()
        {
            var productRevenueStatistics = await _context.BillInfos
                .Where(bi => bi.Bills.DateCheckOut.Year == DateTime.Now.Year) 
                .GroupBy(bi => new { bi.Bills.DateCheckOut.Month, bi.IdFood })
                .Select(g => new ProductStatistics3
                {
                    Month = g.Key.Month,
                    ProductId = g.Key.IdFood,
                    ProductName = g.First().Foods.FoodName,
                    TotalRevenue = g.Sum(bi => bi.TotalPrice),
                    TotalQuantitySold = g.Sum(bi => bi.Count)
                })
                .ToListAsync();

            return productRevenueStatistics;
        }


        // GET: api/BillInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillInfo>>> GetBillInfos()
        {
            var billInfos = await _context.BillInfos
                .Include(bi => bi.Bills)
                .Include(bi => bi.Foods)
                .ToListAsync();

            return billInfos;
        }



        // GET: api/BillInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillInfo>> GetBillInfo(int id)
        {
            var billInfo = await _context.BillInfos
                .Include(bi => bi.Bills)
                .Include(bi => bi.Foods)
                .FirstOrDefaultAsync(bi => bi.BillInfoId == id);

            if (billInfo == null)
            {
                return NotFound();
            }

            return billInfo;
        }
        // POST: api/BillInfo
        [HttpPost]
        public async Task<ActionResult<BillInfo>> PostBillInfo(BillInfoViewModels billInfoViewModel)
        {
            var billExists = await _context.Bills.AnyAsync(b => b.BillId == billInfoViewModel.IdBill);
            var foodExists = await _context.Foods.AnyAsync(f => f.FoodId == billInfoViewModel.IdFood);

            if (!billExists || !foodExists)
            {
                return BadRequest("One or more IDs do not exist.");
            }

            var billInfo = new BillInfo
            {
                IdBill = billInfoViewModel.IdBill,
                IdFood = billInfoViewModel.IdFood,
                Count = billInfoViewModel.Count,
                Price = billInfoViewModel.Price,
                TotalPrice = billInfoViewModel.TotalPrice,
                GiamGia = billInfoViewModel.GiamGia
            };

            var food = await _context.Foods.FindAsync(billInfoViewModel.IdFood);
            if (food == null)
            {
                return NotFound("Food not found.");
            }

            if (food.RemainingQuantity < billInfoViewModel.Count)
            {
                return BadRequest("Not enough remaining quantity for this food.");
            }

            food.RemainingQuantity -= billInfoViewModel.Count;

            if (food.RemainingQuantity < 20)
            {
                food.Status = 0; // Set status to 0 if remaining quantity is less than 20
            }

            _context.BillInfos.Add(billInfo);

            _context.Foods.Update(food);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBillInfo), new { id = billInfo.BillInfoId }, billInfo);
        }


        // PUT: api/BillInfo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillInfo(int id, BillInfoViewModels billInfoViewModel)
        {
           

            var billInfo = await _context.BillInfos.FindAsync(id);

            if (billInfo == null)
            {
                return NotFound();
            }
            
            billInfo.IdBill = billInfoViewModel.IdBill;
            billInfo.IdFood = billInfoViewModel.IdFood;
            billInfo.Count = billInfoViewModel.Count;
            billInfo.Price = billInfoViewModel.Price;
            billInfo.TotalPrice = billInfoViewModel.TotalPrice;
            billInfo.GiamGia = billInfoViewModel.GiamGia;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillInfoExists(id))
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

        // DELETE: api/BillInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillInfo(int id)
        {
            var billInfo = await _context.BillInfos.FindAsync(id);
            if (billInfo == null)
            {
                return NotFound();
            }

            _context.BillInfos.Remove(billInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillInfoExists(int id)
        {
            return _context.BillInfos.Any(e => e.BillInfoId == id);
        }
    }
}
