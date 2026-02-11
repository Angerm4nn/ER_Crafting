using ER_Crafting.Models;
using ER_Crafting.Pages;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static ER_Crafting.Models.Enums;

namespace ER_Crafting.Services
{
 

    public class GameData
    {
        #region declarations & initialisations
        private readonly HttpClient _httpClient;

        private List<Colony> _colonies;
        private List<RawMaterial> _rawMaterials;
        private List<ProductionComponent> _productionComponents;

        private List<Ammunition> _ammunitions;
        private List<Armor> _armors;
        private List<Explosive> _explosives;
        private List<Food> _foods;
        private List<Booster> _boosters;
        private List<Implant> _implants;
        private List<Medication> _medications;
        private List<Miscellaneous> _miscellaneous;
        private List<Weapon> _weapons;
        private List<Clothing> _clothings;

        private List<FinalItem> _allItems;

        private bool _isLoaded = false;

        public GameData()
        {
        }
        #endregion
        #region loading & building
        public async Task LoadAllDataAsync(HttpClient httpClient)
        {
            if (_isLoaded) return; 

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true) }
            };
                _colonies = await httpClient.GetFromJsonAsync<List<Colony>>("data/colonies.json", options);
                _rawMaterials = await httpClient.GetFromJsonAsync<List<RawMaterial>>("data/raw_materials.json", options);
                _productionComponents = await httpClient.GetFromJsonAsync<List<ProductionComponent>>("data/production_components.json", options);
                _ammunitions = await httpClient.GetFromJsonAsync<List<Ammunition>>("data/ammunitions.json", options);
                _armors = await httpClient.GetFromJsonAsync<List<Armor>>("data/armors.json", options);
                _explosives = await httpClient.GetFromJsonAsync<List<Explosive>>("data/explosives.json", options);
                _foods = await httpClient.GetFromJsonAsync<List<Food>>("data/foods.json", options);
                //_boosters = await httpClient.GetFromJsonAsync<List<Booster>>("data/boosters.json", options);
                _implants = await httpClient.GetFromJsonAsync<List<Implant>>("data/implants.json", options);
                _medications = await httpClient.GetFromJsonAsync<List<Medication>>("data/medications.json", options);
                _miscellaneous = await httpClient.GetFromJsonAsync<List<Miscellaneous>>("data/miscellaneous.json", options);
                _weapons = await httpClient.GetFromJsonAsync<List<Weapon>>("data/weapons.json", options);
                //_clothings = await httpClient.GetFromJsonAsync<List<Clothing>>("data/clothings.json", options);

            CreateGlobalListAndID();
            BuildRelations();
            _isLoaded = true;
        }
        #endregion
        #region Handling data
        private void CreateGlobalListAndID()
        {
            var allItems = new IEnumerable<FinalItem>[]
            {
        _ammunitions,
        _armors,
        _explosives,
        _foods,
        //_boosters,
        _implants,
        _medications,
        _miscellaneous,
        _weapons,
        _clothings
            }
            .Where(c => c != null)
            .SelectMany(c => c);

            int id = 1;
            _allItems = new List<FinalItem>();

            foreach (var item in allItems)
            {
                item.Id = id++;
                _allItems.Add(item);
            }
        }

        private void BuildRelations()
        {
            foreach (var item in _allItems)
            {
                foreach (var recipe in item.Recipes)
                {
                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (ingredient.Type == IngredientType.RawMaterial)
                        {
                            GetRawMaterial(ingredient.ItemId).UsedInFinalItems.Add(item);
                        }
                        if (ingredient.Type == IngredientType.ProductionComponent)
                        {
                            var prodComponent = GetProductionComponent(ingredient.ItemId);
                            prodComponent.UsedInFinalItems.Add(item);
                            foreach (var nestedRecipe in prodComponent.Recipes)
                            {
                                foreach (var nestedIngredient in nestedRecipe.Ingredients)
                                {
                                    if (nestedIngredient.Type == IngredientType.ProductionComponent)
                                    {
                                        var nestedProdComponent = GetProductionComponent(nestedIngredient.ItemId);
                                        nestedProdComponent.UsedInFinalItems.Add(item);
                                        nestedProdComponent.UsedInProductionComponents.Add(prodComponent);
                                        foreach (var nestedNestedRecipe in nestedProdComponent.Recipes)
                                        {
                                            foreach (var nestednestedIngredient in nestedNestedRecipe.Ingredients)
                                            {
                                                if (nestednestedIngredient.Type == IngredientType.RawMaterial)
                                                {
                                                    var nestedNestedRawMaterial = GetRawMaterial(nestednestedIngredient.ItemId);
                                                    nestedNestedRawMaterial.UsedInFinalItems.Add(item);
                                                    nestedNestedRawMaterial.UsedInProductionComponents.Add(nestedProdComponent);
                                                    nestedNestedRawMaterial.UsedInProductionComponents.Add(prodComponent);
                                                }
                                                if (nestednestedIngredient.Type == IngredientType.ProductionComponent)
                                                {
                                                    var nestedNestedProdComponent = GetProductionComponent(nestednestedIngredient.ItemId);
                                                    nestedNestedProdComponent.UsedInFinalItems.Add(item);
                                                    nestedNestedProdComponent.UsedInProductionComponents.Add(nestedProdComponent);
                                                    nestedNestedProdComponent.UsedInProductionComponents.Add(prodComponent);
                                                    foreach (var nestedNestedNestedRecipe in nestedNestedProdComponent.Recipes)
                                                    {
                                                        foreach (var nestedNestedNestedIngredient in nestedNestedNestedRecipe.Ingredients)
                                                        {
                                                            var nestedNestedNestedRawMaterial = GetRawMaterial(nestedNestedNestedIngredient.ItemId);
                                                            nestedNestedNestedRawMaterial.UsedInFinalItems.Add(item);
                                                            nestedNestedNestedRawMaterial.UsedInProductionComponents.Add(nestedNestedProdComponent);
                                                            nestedNestedNestedRawMaterial.UsedInProductionComponents.Add(nestedProdComponent);
                                                            nestedNestedNestedRawMaterial.UsedInProductionComponents.Add(prodComponent);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (nestedIngredient.Type == IngredientType.RawMaterial)
                                    {
                                        var nestedRawMaterial = GetRawMaterial(nestedIngredient.ItemId);
                                        nestedRawMaterial.UsedInFinalItems.Add(item);
                                        nestedRawMaterial.UsedInProductionComponents.Add(prodComponent);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region public getters
        public List<Colony> GetAllColonies() => _colonies;
        public List<RawMaterial> GetAllRawMaterials() => _rawMaterials;
        public List<ProductionComponent> GetAllProductionComponents() => _productionComponents;
        public List<FinalItem> GetAllFinalItems() => _allItems;

        public RawMaterial GetRawMaterial(int id) => _rawMaterials?.FirstOrDefault(rm => rm.Id == id);
        public ProductionComponent GetProductionComponent(int id) => _productionComponents?.FirstOrDefault(rm => rm.Id == id);
        #endregion
    }
}
