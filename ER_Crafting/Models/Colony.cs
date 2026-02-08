namespace ER_Crafting.Models
{
    public class Colony
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public List<int> RawMaterialsIds { get; set; } = new List<int>();
        public bool CanProduce { get; set; }

        public float Tax { get; set; }
    }
}
