using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Models
{
    public class FinalItem
    {
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public bool GenderNeutral { get; set; } = true;
        public int ProductionPrice { get; set; }
    }
    public class Ammunition : FinalItem
    {
        public Ammunition()
        {
            Type = ItemType.Ammunition;
        }
    }
    public class Booster : FinalItem
    {
        public Booster()
        {
            Type = ItemType.Booster;
        }
    }

    public class Drug : Booster
    {
        public Drug()
        {
            Type = ItemType.Booster;
        }
        public BoosterType boosterType = BoosterType.Drug;
        
        public float effectDuration { get; set; }
        public float addiction { get; set; }
        public float healthRegeneration { get; set; }
        public float defense { get; set; }          
        public float staminaRegeneration { get; set; }
        public float bioRegeneration { get; set; }
        public float shielding { get; set; }
        public float armor { get; set; }
        public float resistance { get; set; }
        public float criticalOffenseRating { get; set; }
        public float block { get; set; }
        public float weaponRecoil { get; set; }
        public float endurance { get; set; }
        public float staminaDrain { get; set; }
        public float agility { get; set; }
        public float reflection { get; set; }
        public float auraRegeneration { get; set; }
    }
    public class Food : Booster
    {
        public Food()
        {
            Type = ItemType.Booster;
            BoosterType boosterType = BoosterType.Food;
        }
        public BoosterType boosterType { get; set; } = BoosterType.Food;
        public float effectDuration { get; set; }
        public float auraRegeneration { get; set; }
        public float addictionTreatment { get; set; }
        public float staminaRegeneration { get; set; }
        public float criticalOffenseRating { get; set; }
        public float bioRegeneration { get; set; }
        public float weaponRecoil { get; set; }
        public float agility { get; set; }
    }

    public class Armor : FinalItem
    {
        public Armor()
        {
            Type = ItemType.Armor;
        }
        public Faction Faction { get; set; }
        public ArmorType ArmorType { get; set; }
        public float agility { get; set; }
        public float armor { get; set; }
        public float shielding { get; set; }
        public float endurance { get; set; }
        public float resistance { get; set; }
        public float reflection { get; set; }
        public float defenseRating { get; set; }
        public float blockRating { get; set; }
        public float weaponRecoil { get; set; }
        public float healthDrain { get; set; }
        public float bioEnergyDrain { get; set; }
        public float healthRegeneration { get; set; }
        public float bioRegeneration { get; set; }
        public float auraRegeneration { get; set; }
        public float staminaRegeneration { get; set; }
        public float criticalOffenseRating { get; set; }

    }
    public class Explosive : FinalItem
    {
        public Explosive()
        {
            Type = ItemType.Explosive;
        }
        public ExplosiveType explosiveType { get; set; }
        public float attackDelay { get; set; }
        public float damageRadius { get; set; }
        public float energyDamage { get; set; }
        public float bioDamage { get; set; }
        public float destruction { get; set; }
        public float agility { get; set; }
        public float ballisticDamage { get; set; }
        public float auraDamage { get; set;}
        public float xenoDamage { get; set; } 


}
    public class Miscellaneous : FinalItem
    {
        public Miscellaneous()
        {
            Type = ItemType.Miscellaneous;
        }
    }
    public class Implant : FinalItem
    {
        public Implant()
        {
            Type = ItemType.Implant;
        }
        public ImplantType implantType { get; set; }
        public float bioEnergyDrain { get; set; }
        public float agility { get; set; }
        public float staminaRegeneration { get; set; }
        public float healthRegeneration { get; set; }
        public float armor { get; set; }
        public float shielding { get; set; }
    }
    public class Medication : FinalItem
    {
        public Medication()
        {
            Type = ItemType.Medication;
        }
        public float effectDuration { get; set; }
        public float medikitCooldown { get; set; }
        public float healthRegeneration { get; set; }
        public float staminaRegeneration { get; set; }
        public float bioRegeneration { get; set; }
        public float protectionReduction { get; set; }
        public float agility { get; set; }
    }
    public class Weapon : FinalItem
    {
        public Weapon()
        {
            Type = ItemType.Weapon;
        }
        public WeaponType weaponType { get; set; }
        public int clipCapacity { get; set; }
        public float attackDelay {  get; set; }
        public int range { get; set; }
        public float energyDamage { get; set; }
        public float ballisticDamage { get; set; }
        public float staminaDamage { get; set; }
        public float auraDamage { get; set; }
        public float bioDamage { get; set; }
        public float xenoDamage { get; set; }
        public float destruction { get; set; }
        public float weaponRecoil { get; set; }
        public float agility { get; set; }
    }
    public class Clothing : FinalItem
    {
        public Clothing()
        {
            Type = ItemType.Clothing;
        }
        public ClothingType clothingType { get; set; }
    }

}
