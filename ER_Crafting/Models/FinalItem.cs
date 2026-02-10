using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Models
{
    public class FinalItem
    {
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Faction Faction { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public bool GenderNeutral { get; set; } = true;

        public int ProductionCost { get; set; }

    }
}
