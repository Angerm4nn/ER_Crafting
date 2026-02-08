using ER_Crafting.Models;
using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Services
{
    public class ProductionPlanner
    {
        private readonly GameData _gameData;
        private Dictionary<FinalItem, int> _plannedItems = new();
        private Dictionary<FinalItem, Recipe> _selectedRecipes = new();
        private Dictionary<ProductionComponent, Recipe> _selectedComponentRecipes = new();

        public decimal ProductionDiscount { get; set; } = 0m;
        public decimal MiningDiscount { get; set; } = 0m;
        public decimal TransportDiscount { get; set; } = 0m;

        public event Action? OnProductionChanged;

        public ProductionPlanner(GameData gameData)
        {
            _gameData = gameData;
        }

        public Dictionary<FinalItem, int> GetPlannedItems() => _plannedItems;

        public Recipe? GetSelectedRecipe(FinalItem item)
        {
            return _selectedRecipes.ContainsKey(item) ? _selectedRecipes[item] : null;
        }

        public Recipe? GetSelectedComponentRecipe(ProductionComponent component)
        {
            return _selectedComponentRecipes.ContainsKey(component) ? _selectedComponentRecipes[component] : null;
        }

        public Recipe GetEffectiveRecipe(ProductionComponent component)
        {
            if (_selectedComponentRecipes.ContainsKey(component))
                return _selectedComponentRecipes[component];

            return component.Recipes.Count > 0 ? component.Recipes[0] : null;
        }

        public void SetSelectedRecipe(FinalItem item, Recipe recipe)
        {
            _selectedRecipes[item] = recipe;
        }

        public void SetSelectedComponentRecipe(ProductionComponent component, Recipe recipe)
        {
            _selectedComponentRecipes[component] = recipe;
        }

        public void AddItem(FinalItem item, int quantity = 1)
        {
            if (_plannedItems.ContainsKey(item))
            {
                _plannedItems[item] += quantity;
            }
            else
            {
                _plannedItems[item] = quantity;
            }
            OnProductionChanged?.Invoke();
        }

        public void RemoveItem(FinalItem item)
        {
            _plannedItems.Remove(item);
            _selectedRecipes.Remove(item);
            OnProductionChanged?.Invoke();
        }

        public void UpdateQuantity(FinalItem item, int quantity)
        {
            if (quantity <= 0)
                RemoveItem(item);
            else
            {
                _plannedItems[item] = quantity;
                OnProductionChanged?.Invoke();
            }
        }

        public void Clear()
        {
            _plannedItems.Clear();
            _selectedRecipes.Clear();
            _selectedComponentRecipes.Clear();
            OnProductionChanged?.Invoke();
        }

        public Dictionary<RawMaterial, int> GetTotalRawMaterials()
        {
            var totals = new Dictionary<RawMaterial, int>();

            foreach (var kvp in _plannedItems)
            {
                var finalItem = kvp.Key;
                var quantity = kvp.Value;

                var recipe = GetSelectedRecipe(finalItem);
                if (recipe != null)
                {
                    CalculateRecipeRawMaterials(recipe, quantity, totals);
                }
            }

            return totals;
        }

        private void CalculateRecipeRawMaterials(Recipe recipe, int multiplier, Dictionary<RawMaterial, int> totals)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Type == IngredientType.RawMaterial)
                {
                    var rawMaterial = _gameData.GetRawMaterial(ingredient.ItemId);
                    if (rawMaterial != null)
                    {
                        int needed = ingredient.Quantity * multiplier;
                        if (totals.ContainsKey(rawMaterial))
                            totals[rawMaterial] += needed;
                        else
                            totals[rawMaterial] = needed;
                    }
                }
                else if (ingredient.Type == IngredientType.ProductionComponent)
                {
                    var component = _gameData.GetProductionComponent(ingredient.ItemId);
                    if (component != null)
                    {
                        var componentRecipe = GetEffectiveRecipe(component);
                        if (componentRecipe != null)
                        {
                            int needed = ingredient.Quantity * multiplier;
                            CalculateRecipeRawMaterials(componentRecipe, needed, totals);
                        }
                    }
                }
            }
        }

        public Dictionary<ProductionComponent, int> GetTotalProductionComponents()
        {
            var totals = new Dictionary<ProductionComponent, int>();

            foreach (var kvp in _plannedItems)
            {
                var finalItem = kvp.Key;
                var quantity = kvp.Value;

                var recipe = GetSelectedRecipe(finalItem);
                if (recipe != null)
                {
                    var timesToCraft = (int)Math.Ceiling((decimal)quantity / recipe.OutputQuantity);
                    AddProductionComponentsFromRecipe(recipe, timesToCraft, totals);
                }
            }

            return totals;
        }

        private void AddProductionComponentsFromRecipe(Recipe recipe, int multiplier, Dictionary<ProductionComponent, int> totals)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Type == IngredientType.ProductionComponent)
                {
                    var component = GetProductionComponentById(ingredient.ItemId);
                    if (component != null)
                    {
                        int needed = ingredient.Quantity * multiplier;

                        if (totals.ContainsKey(component))
                            totals[component] += needed;
                        else
                            totals[component] = needed;

                        var componentRecipe = GetEffectiveRecipe(component);
                        if (componentRecipe != null)
                        {
                            AddProductionComponentsFromRecipe(componentRecipe, needed, totals);
                        }
                    }
                }
            }
        }

        private ProductionComponent GetProductionComponentById(int id)
        {
            return _gameData.GetProductionComponent(id);
        }

        public void NotifyColonyTaxChanged()
        {
            OnProductionChanged?.Invoke();
        }

        public decimal ApplyProductionDiscount(decimal baseCost)
        {
            return baseCost * (1 - ProductionDiscount / 100m);
        }

        public decimal ApplyMiningDiscount(decimal baseCost)
        {
            return baseCost * (1 - MiningDiscount / 100m);
        }

        public decimal ApplyTransportDiscount(decimal baseCost)
        {
            return baseCost * (1 - TransportDiscount / 100m);
        }
    }
}