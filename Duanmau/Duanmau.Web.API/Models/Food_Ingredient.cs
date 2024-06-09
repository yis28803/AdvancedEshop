namespace Duanmau.Web.API.Models
{
    public class Food_Ingredient
    {
        public int Food_IngredientId { get; set; }
        public int FoodId { get; set; }
        public int IngredientId { get; set; }
        public int SoLuong { get; set;}
        public Food? Foods { get; set; }
        public Ingredient? Ingredients { get; set;}
    }
}
