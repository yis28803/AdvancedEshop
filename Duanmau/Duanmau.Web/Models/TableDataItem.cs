namespace Duanmau.Web.Models
{

    public class CheckoutItem
    {
        public int Key { get; set; }
        public List<FoodItem>? FoodItems { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class FoodItem
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int IDFood { get; set; }
    }
    public class BillAndBillInfoViewModel
    {
        public DateTime DateCheckIn { get; set; }
        public DateTime DateCheckOut { get; set; }
        public int IdNhanVien { get; set; }
        public int IdKhachHang { get; set; }
        public int IdTableFood { get; set; }
        public decimal TotalValue { get; set; }
        public List<BillInfoViewModel>? BillInfos { get; set; }
    }

    public class BillInfoViewModel
    {
        public int IdFood { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GiamGia { get; set; }
    }

}
