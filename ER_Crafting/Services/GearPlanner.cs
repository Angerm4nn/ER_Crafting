using ER_Crafting.Models;
using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Services
{
    public class GearPlanner
    {
        //BaseStats
        private const double BASE_HEALTH_REGEN = 0.7;
        private const double BASE_STAMINA_REGEN = 2.0;
        private const double BASE_AURA_REGEN = 2.5;
        //MainStats
        public double Agility { get; private set; } = 100.0;
        public double Addiction { get; private set; } = 0.0;
        //Regeneration
        public double HealthRegeneration { get; private set; } = 0.7;
        public double StaminaRegeneration { get; private set; } = 2.0;
        public double BioRegeneration { get; private set; } = 0.0;
        public double AuraRegeneration { get; private set; } = 2.5;
        public double AddictionTreatment { get; private set; } = 0.0;
        //Damages
        public double HealthDrain { get; private set; } = 0.0;
        public double StaminaDrain { get; private set; } = 0.0;
        public double BioEnergyDrain { get; private set; } = 0.0;
        public double AuraLoss { get; private set; } = 0.0;
        public double CriticalOffenseRating { get; private set; } = 0.0;
        public double WeaponRecoil { get; private set; } = 0.0;
        //Protection
        public double ArmorValue { get; private set; } = 0.0;
        public double Shielding { get; private set; } = 0.0;
        public double Endurance { get; private set; } = 0.0;
        public double Resistance { get; private set; } = 0.0;
        public double Reflection { get; private set; } = 0.0;
        public double DefenseRating { get; private set; } = 0.0;
        public double BlockRating { get; private set; } = 0.0;
        public double ProtectionReduction { get; private set; } = 0.0;

        //Armor Slots
        public FinalItem? Helmet { get; private set; }
        public FinalItem? ShoulderPads { get; private set; }
        public FinalItem? ArmPads { get; private set; }
        public FinalItem? TorsoArmor { get; private set; }
        public FinalItem? LegPads { get; private set; }
        public FinalItem? Gloves { get; private set; }

        //Implants
        public FinalItem? LegImplant { get; private set; }
        public FinalItem? ChestImplant { get; private set; }
        public FinalItem? ShoulderImplant { get; private set; }
        public FinalItem? BackImplant { get; private set; }
        public FinalItem? EyesImplant { get; private set; }
        public FinalItem? ActiveImplant { get; private set; }

        //Weapons
        public FinalItem? FirstWeapon { get; private set; }

        //Consumables
        public FinalItem? FirstBooster { get; private set; }
        public FinalItem? SecondBooster { get; private set; }
        public FinalItem? FirstMedication { get; private set; }
        public FinalItem? SecondMedication { get; private set; }

        public event Action? OnGearChanged;

        public bool IsEquippable(FinalItem item)
        {
            return item.Type switch
            {
                ItemType.Weapon => true,
                ItemType.Implant => true,
                ItemType.Booster => true,
                ItemType.Medication => true,
                ItemType.Armor => true,
                _ => false
            };
        }

        public void Equip(FinalItem item)
        {
            switch (item.Type)
            {
                case ItemType.Weapon:
                    FirstWeapon = item;
                    break;

                case ItemType.Implant:
                    Implant implant = item as Implant;

                    // Unequip ALL other implants first - only ONE implant allowed
                    EyesImplant = null;
                    ChestImplant = null;
                    BackImplant = null;
                    ShoulderImplant = null;
                    LegImplant = null;

                    // Now equip the new implant
                    if (implant.implantType == ImplantType.EyesImplant)
                    {
                        EyesImplant = implant;
                    }
                    else if (implant.implantType == ImplantType.ChestImplant)
                    {
                        ChestImplant = implant;
                        Unequip(TorsoArmor); // Still remove conflicting armor
                    }
                    else if (implant.implantType == ImplantType.BackImplant)
                    {
                        BackImplant = implant;
                        Unequip(TorsoArmor); // Still remove conflicting armor
                    }
                    else if (implant.implantType == ImplantType.ShoulderImplant)
                    {
                        ShoulderImplant = implant;
                        Unequip(ShoulderPads); // Still remove conflicting armor
                    }
                    else if (implant.implantType == ImplantType.LegImplant)
                    {
                        LegImplant = implant;
                        Unequip(LegPads); // Still remove conflicting armor
                    }
                    break;

                case ItemType.Booster:
                    if (FirstBooster == null)
                        FirstBooster = item;
                    else if (SecondBooster == null)
                        SecondBooster = item;
                    else
                        FirstBooster = item;
                    break;

                case ItemType.Medication:
                    if (FirstMedication == null)
                        FirstMedication = item;
                    else if (SecondMedication == null)
                        SecondMedication = item;
                    else
                        FirstMedication = item;
                    break;

                case ItemType.Armor:
                    Armor armor = item as Armor;
                    if (armor.ArmorType == ArmorType.Helmet)
                    {
                        Helmet = armor;
                    }
                    else if (armor.ArmorType == ArmorType.ShoulderPads)
                    {
                        ShoulderPads = armor;
                        Unequip(ShoulderImplant); // Mutual exclusivity
                    }
                    else if (armor.ArmorType == ArmorType.ArmPads)
                    {
                        ArmPads = armor;
                    }
                    else if (armor.ArmorType == ArmorType.TorsoArmor)
                    {
                        TorsoArmor = armor;
                        Unequip(BackImplant); // Mutual exclusivity
                        Unequip(ChestImplant); // Mutual exclusivity
                    }
                    else if (armor.ArmorType == ArmorType.LegPads)
                    {
                        LegPads = armor;
                        Unequip(LegImplant); // Mutual exclusivity
                    }
                    else if (armor.ArmorType == ArmorType.Gloves)
                    {
                        Gloves = armor;
                    }
                    break;

                default:
                    return;
            }

            CalculateStats();
            OnGearChanged?.Invoke();
        }

        public void Unequip(FinalItem? item)
        {
            if (item == null) return;

            if (Helmet == item) Helmet = null;
            else if (ShoulderPads == item) ShoulderPads = null;
            else if (ArmPads == item) ArmPads = null;
            else if (TorsoArmor == item) TorsoArmor = null;
            else if (LegPads == item) LegPads = null;
            else if (Gloves == item) Gloves = null;
            else if (EyesImplant == item) EyesImplant = null;
            else if (ShoulderImplant == item) ShoulderImplant = null;
            else if (BackImplant == item) BackImplant = null;
            else if (ChestImplant == item) ChestImplant = null;
            else if (LegImplant == item) LegImplant = null;
            else if (FirstWeapon == item) FirstWeapon = null;
            else if (FirstBooster == item) FirstBooster = null;
            else if (SecondBooster == item) SecondBooster = null;
            else if (FirstMedication == item) FirstMedication = null;
            else if (SecondMedication == item) SecondMedication = null;
            else
                return;

            CalculateStats();
            OnGearChanged?.Invoke();
        }

        public void UnequipSlot(string slotName)
        {
            switch (slotName)
            {
                case "Helmet": Helmet = null; break;
                case "ShoulderPads": ShoulderPads = null; break;
                case "ArmPads": ArmPads = null; break;
                case "TorsoArmor": TorsoArmor = null; break;
                case "LegPads": LegPads = null; break;
                case "Gloves": Gloves = null; break;
                case "LegImplant": LegImplant = null; break;
                case "ChestImplant": ChestImplant = null; break;
                case "ShoulderImplant": ShoulderImplant = null; break;
                case "BackImplant": BackImplant = null; break;
                case "EyesImplant": EyesImplant = null; break;
                case "FirstWeapon": FirstWeapon = null; break;
                case "FirstBooster": FirstBooster = null; break;
                case "SecondBooster": SecondBooster = null; break;
                case "FirstMedication": FirstMedication = null; break;
                case "SecondMedication": SecondMedication = null; break;
            }
            CalculateStats();
            OnGearChanged?.Invoke();
        }

        public void ClearAll()
        {
            Helmet = ShoulderPads = ArmPads = TorsoArmor = LegPads = Gloves = null;
            LegImplant = ChestImplant = ShoulderImplant = BackImplant = EyesImplant = null;
            FirstWeapon = null;
            FirstBooster = SecondBooster = FirstMedication = SecondMedication = null;
            CalculateStats();
            OnGearChanged?.Invoke();
        }

        private void CalculateStats()
        {
            Agility = 100.0;
            Addiction = 0.0;
            HealthRegeneration = BASE_HEALTH_REGEN;
            StaminaRegeneration = BASE_STAMINA_REGEN;
            BioRegeneration = 0.0;
            AuraRegeneration = BASE_AURA_REGEN;
            AddictionTreatment = 0.0;
            HealthDrain = 0.0;
            StaminaDrain = 0.0;
            BioEnergyDrain = 0.0;
            AuraLoss = 0.0;
            CriticalOffenseRating = 0.0;
            WeaponRecoil = 0.0;
            ArmorValue = 0.0;
            Shielding = 0.0;
            Endurance = 0.0;
            Resistance = 0.0;
            Reflection = 0.0;
            DefenseRating = 0.0;
            BlockRating = 0.0;
            ProtectionReduction = 0.0;

            var allItems = GetAllEquippedItems();

            foreach (var item in allItems)
            {
                ApplyItemStats(item);
            }
        }

        private List<FinalItem> GetAllEquippedItems()
        {
            var items = new List<FinalItem>();

            // Armor
            if (Helmet != null) items.Add(Helmet);
            if (ShoulderPads != null) items.Add(ShoulderPads);
            if (ArmPads != null) items.Add(ArmPads);
            if (TorsoArmor != null) items.Add(TorsoArmor);
            if (LegPads != null) items.Add(LegPads);
            if (Gloves != null) items.Add(Gloves);

            // Implants
            if (LegImplant != null) items.Add(LegImplant);
            if (ChestImplant != null) items.Add(ChestImplant);
            if (ShoulderImplant != null) items.Add(ShoulderImplant);
            if (BackImplant != null) items.Add(BackImplant);
            if (EyesImplant != null) items.Add(EyesImplant);

            // Weapon
            if (FirstWeapon != null) items.Add(FirstWeapon);

            // Consumables
            if (FirstBooster != null) items.Add(FirstBooster);
            if (SecondBooster != null) items.Add(SecondBooster);
            if (FirstMedication != null) items.Add(FirstMedication);
            if (SecondMedication != null) items.Add(SecondMedication);

            return items;
        }

        private void ApplyItemStats(FinalItem item)
        {
            switch (item)
            {
                case Armor armor:
                    ApplyArmorStats(armor);
                    break;
                case Weapon weapon:
                    ApplyWeaponStats(weapon);
                    break;
                case Drug drug:
                    ApplyDrugStats(drug);
                    break;
                case Food food:
                    ApplyFoodStats(food);
                    break;
                case Medication medication:
                    ApplyMedicationStats(medication);
                    break;
                case Implant implant:
                    ApplyImplantStats(implant);
                    break;
            }
        }

        private void ApplyArmorStats(Armor armor)
        {
            Agility += armor.agility;
            ArmorValue += armor.armor;
            Shielding += armor.shielding;
            Endurance += armor.endurance;
            Resistance += armor.resistance;
            Reflection += armor.reflection;
            DefenseRating += armor.defenseRating;
            BlockRating += armor.blockRating;
            WeaponRecoil += armor.weaponRecoil;
            HealthDrain += armor.healthDrain;
            BioEnergyDrain += armor.bioEnergyDrain;
            HealthRegeneration += armor.healthRegeneration;
            BioRegeneration += armor.bioRegeneration;
            AuraRegeneration += armor.auraRegeneration;
            StaminaRegeneration += armor.staminaRegeneration;
            CriticalOffenseRating += armor.criticalOffenseRating;
            Addiction += armor.addiction;
            AddictionTreatment += armor.addictionTreatment;
            ProtectionReduction += armor.protectionReduction;
        }

        private void ApplyWeaponStats(Weapon weapon)
        {
            WeaponRecoil += weapon.weaponRecoil;
            Agility += weapon.agility;
        }

        private void ApplyDrugStats(Drug drug)
        {
            Addiction += drug.addiction;
            HealthRegeneration += drug.healthRegeneration;
            StaminaRegeneration += drug.staminaRegeneration;
            BioRegeneration += drug.bioRegeneration;
            AuraRegeneration += drug.auraRegeneration;
            Shielding += drug.shielding;
            ArmorValue += drug.armor;
            Resistance += drug.resistance;
            CriticalOffenseRating += drug.criticalOffenseRating;
            BlockRating += drug.block;
            WeaponRecoil += drug.weaponRecoil;
            Endurance += drug.endurance;
            StaminaDrain += drug.staminaDrain;
            Agility += drug.agility;
            Reflection += drug.reflection;
            DefenseRating += drug.defense;
        }

        private void ApplyFoodStats(Food food)
        {
            AuraRegeneration += food.auraRegeneration;
            AddictionTreatment += food.addictionTreatment;
            StaminaRegeneration += food.staminaRegeneration;
            CriticalOffenseRating += food.criticalOffenseRating;
            BioRegeneration += food.bioRegeneration;
            WeaponRecoil += food.weaponRecoil;
            Agility += food.agility;
        }

        private void ApplyMedicationStats(Medication medication)
        {
            HealthRegeneration += medication.healthRegeneration;
            StaminaRegeneration += medication.staminaRegeneration;
            BioRegeneration += medication.bioRegeneration;
            ProtectionReduction += medication.protectionReduction;
            Agility += medication.agility;
        }

        private void ApplyImplantStats(Implant implant)
        {
            BioEnergyDrain += implant.bioEnergyDrain;
            Agility += implant.agility;
            StaminaRegeneration += implant.staminaRegeneration;
            HealthRegeneration += implant.healthRegeneration;
            ArmorValue += implant.armor;
            Shielding += implant.shielding;
        }
    }
}
