namespace Duanmau.Web.API.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string? FoodName { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string? Mota { get; set; }
        public int RemainingQuantity { get; set; }
        public string? File { get; set; }
        public int DanhMucFoodId { get; set; }
        public DanhMucFood? DanhMucFoods { get; set; }
    }
}
