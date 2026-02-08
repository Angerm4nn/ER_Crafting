using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Models
{
    public class RecipeIngredient
    {
        public IngredientType Type { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public Dictionary<RecipeIngredient, int> TotalRawMaterials { get; set; } = new();
        public Dictionary<RecipeIngredient, int> TotalProductionComponent { get; set; } = new();
    }
}
