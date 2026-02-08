namespace ER_Crafting.Models
{
    public class ProductionComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public int RefiningCost { get; set; }

        public HashSet<ProductionComponent> UsedInProductionComponents { get; set; } = new HashSet<ProductionComponent>();
        public HashSet<FinalItem> UsedInFinalItems { get; set; } = new HashSet<FinalItem>();
    }
}
