namespace ER_Crafting.Models
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int MiningCost { get; set; }

        public HashSet<ProductionComponent> UsedInProductionComponents { get; set; } = new HashSet<ProductionComponent>();
        public HashSet<FinalItem> UsedInFinalItems { get; set; } = new HashSet<FinalItem>();
    }
}
