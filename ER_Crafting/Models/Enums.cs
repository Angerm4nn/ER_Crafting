namespace ER_Crafting.Models
{
    public class Enums
    {
        public enum IngredientType
        {
            RawMaterial,
            ProductionComponent
        }
        public enum ItemType
        {
            Ammunition,
            Armor,
            Explosive,
            Booster,
            Implant,
            Medication,
            Miscellaneous,
            Weapon,
            Clothing
        }
        public enum BoosterType
        {
            Drug,
            Food
        }
        public enum ArmorType
        {
            Helmet,
            ShoulderPads,
            TorsoArmor,
            ArmPads,
            LegPads,
            Gloves
        }
        public enum ImplantType
        {
            EyesImplant,
            ShoulderImplant,
            ChestImplant,
            BackImplant,
            LegImplant
        }
        public enum WeaponType
        {
            Energy,
            Ballistic,
            Bio,
            Healing,
            Stun,
            Other
        }
        public enum ClothingType
        {
            Glasses,
            Shoes,
            LegClothing,
        }
        public enum ExplosiveType
        {
            Ballistic,
            Bio
        }
        public enum Faction
        {
            Civilian,
            LED,
            FDC,
            GOM,
            BOS,
            MOTB,
            CMG,
            EC,
            VI
        }
    }
}
