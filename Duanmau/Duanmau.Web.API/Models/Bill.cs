namespace Duanmau.Web.API.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public int IdTableFood { get; set; }
        public int IdNhanVien { get; set; }
        public int IdKhachHang { get; set; }
        public decimal TotalValue { get; set; }
        public KhachHang? KhachHangs { get; set; }
        public NhanVien? NhanViens { get; set; }
        public Tablefood? Tablefoods { get; set; }
    }
    public class BillViewModels
    {
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public int IdTableFood { get; set; }
        public int IdNhanVien { get; set; }
        public int IdKhachHang { get; set; }
        public decimal TotalValue { get; set; }
    }
    public class BillViewModels2
    {
        public int BillId { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public int IdTableFood { get; set; }
        public int IdNhanVien { get; set; }
        public int IdKhachHang { get; set; }
        public decimal TotalValue { get; set; }
        public string? TenKhachHang { get; set; }
        public string? TenNhanVien { get; set; }
        public string? TenTableFood { get; set; }
        public List<BillInfoViewModels>? BillInfoViewModels { get; set; }
    }
    public class BillViewModels3
    {
        public int BillId { get; set; }
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public int IdTableFood { get; set; }
        public int IdNhanVien { get; set; }
        public int IdKhachHang { get; set; }
        public decimal TotalValue { get; set; }
        public string? TenKhachHang { get; set; }
        public string? TenNhanVien { get; set; }
        public string? TenTableFood { get; set; }
        public List<BillInfoViewModels2>? BillInfoViewModels { get; set; }
    }

}
