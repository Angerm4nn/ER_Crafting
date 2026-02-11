using ER_Crafting.Models;
using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Services
{
    public class GearPlanner
    {
        //MainStats
        public float Agility { get; private set; } = 100.0f;
        public float Addiction { get; private set; } = 0.0f;
        //Regeneration
        public float HealthRegeneration { get; private set; } = 0.7f;
        public float StaminaRegeneration { get; private set; } = 2.0f;
        public float BioRegeneration { get; private set; } = 0.0f;
        public float AuraRegeneration { get; private set; } = 2.5f;
        public float AddictionTreatment { get; private set; } = 0.0f;
        //Damages
        public float HealthDrain { get; private set; } = 0.0f;
        public float StaminaDrain { get; private set; } = 0.0f;
        public float BioEnergyDrain { get; private set; } = 0.0f;
        public float AuraLoss { get; private set; } = 0.0f;
        public float CriticalOffenseRating { get; private set; } = 0.0f;
        public float WeaponRecoil { get; private set; } = 0.0f;
        //Protection
        public float ArmorValue { get; private set; } = 0.0f;
        public float Shielding { get; private set; } = 0.0f;
        public float Endurance { get; private set; } = 0.0f;
        public float Resistance { get; private set; } = 0.0f;
        public float Reflection { get; private set; } = 0.0f;
        public float DefenseRating { get; private set; } = 0.0f;
        public float BlockRating { get; private set; } = 0.0f;
        public float ProtectionReduction { get; private set; } = 0.0f;

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

        //Weapons
        public FinalItem? FirstWeapon { get; private set; }
        public FinalItem? SecondWeapon { get; private set; }
        public FinalItem? ThirdWeapon { get; private set; }
        public FinalItem? ActiveWeapon { get; private set; }

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
                    if (FirstWeapon == null)
                        FirstWeapon = item;
                    else if (SecondWeapon == null)
                        SecondWeapon = item;
                    else if (ThirdWeapon == null)
                        ThirdWeapon = item;
                    else
                        FirstWeapon = item;
                    break;

                case ItemType.Implant:
                    Implant implant = item as Implant;
                    if(implant.implantType == ImplantType.EyesImplant)
                    {
                        EyesImplant = implant;
                    }
                    if (implant.implantType == ImplantType.ChestImplant)
                    {
                        ChestImplant = implant;
                    }
                    if (implant.implantType == ImplantType.BackImplant)
                    {
                        BackImplant = implant;
                    }
                    if (implant.implantType == ImplantType.ShoulderImplant)
                    {
                        ShoulderImplant = implant;
                    }
                    if (implant.implantType == ImplantType.LegImplant)
                    {
                        LegImplant = implant;
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
                    if (Helmet == null)
                        Helmet = item;
                    else if (ShoulderPads == null)
                        ShoulderPads = item;
                    else if (ArmPads == null)
                        ArmPads = item;
                    else if (TorsoArmor == null)
                        TorsoArmor = item;
                    else if (LegPads == null)
                        LegPads = item;
                    else if (Gloves == null)
                        Gloves = item;
                    else
                        Helmet = item;
                    break;

                default:
                    return;
            }

            OnGearChanged?.Invoke();
        }

        public void Unequip(FinalItem item)
        {
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
            else if (SecondWeapon == item) SecondWeapon = null;
            else if (ThirdWeapon == item) ThirdWeapon = null;
            else if (FirstBooster == item) FirstBooster = null;
            else if (SecondBooster == item) SecondBooster = null;
            else if (FirstMedication == item) FirstMedication = null;
            else if (SecondMedication == item) SecondMedication = null;
            else
                return; // Item not found, don't trigger OnGearChanged

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
                case "SecondWeapon": SecondWeapon = null; break;
                case "ThirdWeapon": ThirdWeapon = null; break;
                case "FirstBooster": FirstBooster = null; break;
                case "SecondBooster": SecondBooster = null; break;
                case "FirstMedication": FirstMedication = null; break;
                case "SecondMedication": SecondMedication = null; break;
            }
            OnGearChanged?.Invoke();
        }

        public void ClearAll()
        {
            Helmet = ShoulderPads = ArmPads = TorsoArmor = LegPads = Gloves = null;
            LegImplant = ChestImplant = ShoulderImplant = BackImplant = EyesImplant = null;
            FirstWeapon = SecondWeapon = ThirdWeapon = ActiveWeapon = null;
            FirstBooster = SecondBooster = FirstMedication = SecondMedication = null;
            OnGearChanged?.Invoke();
        }
    }
}