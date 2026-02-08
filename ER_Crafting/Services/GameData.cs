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
        private List<FinalItem> _finalItems;

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
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
            };


            _colonies = await httpClient.GetFromJsonAsync<List<Colony>>("data/colonies.json", options);
            _rawMaterials = await httpClient.GetFromJsonAsync<List<RawMaterial>>("data/raw_materials.json", options);
            _productionComponents = await httpClient.GetFromJsonAsync<List<ProductionComponent>>("data/production_components.json", options);
            _finalItems = await httpClient.GetFromJsonAsync<List<FinalItem>>("data/final_items.json", options);

            BuildRelations();
            _isLoaded = true;
        }
        #endregion
        #region RawMaterials
        public void BuildRelations()
        {
            foreach (var item in _finalItems)
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
        }
        #endregion
        #region public getters
        public List<Colony> GetAllColonies() => _colonies;
        public List<RawMaterial> GetAllRawMaterials() => _rawMaterials;
        public List<ProductionComponent> GetAllProductionComponents() => _productionComponents;
        public List<FinalItem> GetAllFinalItems() => _finalItems;

        public RawMaterial GetRawMaterial(int id) => _rawMaterials?.FirstOrDefault(rm => rm.Id == id);
        public ProductionComponent GetProductionComponent(int id) => _productionComponents?.FirstOrDefault(rm => rm.Id == id);
        #endregion
    }
}
