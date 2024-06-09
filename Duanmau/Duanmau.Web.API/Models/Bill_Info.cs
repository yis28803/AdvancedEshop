namespace Duanmau.Web.API.Models
{
    public class BillInfo
    {
        public int BillInfoId { get; set; }
        public int IdBill { get; set; }
        public int IdFood { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GiamGia { get; set; }
        public Bill? Bills { get; set; }
        public Food? Foods { get; set; }
    }
    public class BillInfoViewModels
    {
        public int IdBill { get; set; }
        public int IdFood { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GiamGia { get; set; }
    }
    public class BillInfoViewModels2
    {
        public int IdBill { get; set; }
        public string? FoodName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GiamGia { get; set; }
    }

}
