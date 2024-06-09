using Duanmau.Web.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Duanmau.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/Bill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills()
        {
            var bills = await _context.Bills
                .Include(b => b.Tablefoods)
                .Include(b => b.NhanViens)
                .Include(b => b.KhachHangs)
                .ToListAsync();

            return bills;
        }
        // GET: api/Bill/RevenueStatistics
        [HttpGet("RevenueStatistics")]
        public async Task<ActionResult<IEnumerable<RevenueStatistics>>> GetRevenueStatistics()
        {
            var revenueStatistics = await _context.Bills
                .GroupBy(b => b.DateCheckOut.Date)
                .Select(g => new RevenueStatistics
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(b => b.TotalValue)
                })
                .ToListAsync();

            return revenueStatistics;
        }

        // GET: api/Bill
        [HttpGet("xuatbill")]
        public async Task<ActionResult<IEnumerable<BillViewModels3>>> GetBillxuatbills()
        {
            var bills = await _context.Bills
                .Include(b => b.Tablefoods)
                .Include(b => b.NhanViens)
                .Include(b => b.KhachHangs)
                .ToListAsync();

            var billViewModels = bills.Select(b => new BillViewModels3
            {
                BillId = b.BillId,
                DateCheckIn = b.DateCheckIn,
                DateCheckOut = b.DateCheckOut,
                IdTableFood = b.IdTableFood,
                IdNhanVien = b.IdNhanVien,
                IdKhachHang = b.IdKhachHang,
                TenKhachHang = b.KhachHangs.Name,
                TenNhanVien = b.NhanViens.Name,
                TenTableFood = b.Tablefoods.TablefoodName,
                TotalValue = b.TotalValue,
                BillInfoViewModels = _context.BillInfos
                    .Where(bi => bi.IdBill == b.BillId)
                    .Select(bi => new BillInfoViewModels2
                    {
                        IdBill = bi.IdBill,
                        FoodName = bi.Foods.FoodName,
                        Count = bi.Count,
                        Price = bi.Price,
                        TotalPrice = bi.TotalPrice,
                        GiamGia = bi.GiamGia
                    })
                    .ToList()
            }).ToList();
            return billViewModels;
        }
        // GET: api/Bill/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bill>> GetBill(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Tablefoods)
                .Include(b => b.NhanViens)
                .Include(b => b.KhachHangs)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }
        // POST: api/Bill
        [HttpPost]
        public async Task<ActionResult<Bill>> PostBill(BillViewModels billViewModel)
        {
            var tableExists = await _context.Tablefoods.AnyAsync(t => t.TablefoodId == billViewModel.IdTableFood);
            var employeeExists = await _context.NhanViens.AnyAsync(e => e.Id == billViewModel.IdNhanVien);
            var customerExists = await _context.KhachHangs.AnyAsync(c => c.Id == billViewModel.IdKhachHang);

            if (!tableExists || !employeeExists || !customerExists)
            {
                return BadRequest("One or more IDs do not exist.");
            }

            var bill = new Bill
            {
                DateCheckIn = billViewModel.DateCheckIn,
                DateCheckOut = billViewModel.DateCheckOut,
                IdTableFood = billViewModel.IdTableFood,
                IdNhanVien = billViewModel.IdNhanVien,
                IdKhachHang = billViewModel.IdKhachHang,
                TotalValue = billViewModel.TotalValue
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBill), new { id = bill.BillId }, bill);
        }
        // PUT: api/Bill/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, BillViewModels billViewModel)
        {
            

            var tableExists = await _context.Tablefoods.AnyAsync(t => t.TablefoodId == billViewModel.IdTableFood);
            var employeeExists = await _context.NhanViens.AnyAsync(e => e.Id == billViewModel.IdNhanVien);
            var customerExists = await _context.KhachHangs.AnyAsync(c => c.Id == billViewModel.IdKhachHang);

            if (!tableExists || !employeeExists || !customerExists)
            {
                return BadRequest("One or more IDs do not exist.");
            }

            var bill = await _context.Bills.FindAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            // Update bill properties
            bill.DateCheckIn = billViewModel.DateCheckIn;
            bill.DateCheckOut = billViewModel.DateCheckOut;
            bill.IdTableFood = billViewModel.IdTableFood;
            bill.IdNhanVien = billViewModel.IdNhanVien;
            bill.IdKhachHang = billViewModel.IdKhachHang;
            bill.TotalValue = billViewModel.TotalValue;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
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

        // DELETE: api/Bill/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            // Tìm hóa đơn dựa trên id
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy hóa đơn
            }

            // Tìm tất cả các BillInfo có BillId tương ứng và xóa chúng
            var billInfos = await _context.BillInfos.Where(bi => bi.IdBill == id).ToListAsync();
            _context.BillInfos.RemoveRange(billInfos);
            await _context.SaveChangesAsync();

            // Sau khi xóa tất cả BillInfo liên quan, xóa hóa đơn
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return NoContent(); // Trả về 204 No Content nếu xóa thành công
        }


        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }

    }
}
