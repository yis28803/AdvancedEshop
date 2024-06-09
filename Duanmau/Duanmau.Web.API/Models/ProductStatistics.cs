namespace Duanmau.Web.API.Models
{
    public class ProductStatistics
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalQuantitySold { get; set; }
    }
    public class ProductStatistics2
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalQuantitySold { get; set; }
    }


    public class ProductStatistics3
    {
        public int Month { get; set; } // Tháng
        public int ProductId { get; set; } // ID của sản phẩm
        public string? ProductName { get; set; } // Tên của sản phẩm
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
        public int TotalQuantitySold { get; set; } // Tổng số lượng đã bán
    }


    // GET: api/Food/ProductStatistics
    /*[HttpGet("ProductStatistics")]
    public async Task<ActionResult<IEnumerable<ProductStatistics>>> GetProductStatistics()
    {
        var productStatistics = await _context.Foods
            .GroupBy(f => f.DanhMucFoodId)
            .Select(g => new ProductStatistics
            {
                CategoryId = g.Key,
                CategoryName = g.FirstOrDefault().DanhMucFoods.DanhMucFoodName,
                TotalRevenue = g.Sum(f => f.Price * f.RemainingQuantity),
                TotalQuantitySold = g.Sum(f => f.RemainingQuantity)
            })
            .ToListAsync();

        return productStatistics;
    }*/
}
