using ER_Crafting;
using ER_Crafting.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<GameData>();
builder.Services.AddSingleton<GearPlanner>();

builder.Services.AddSingleton<ProductionPlanner>();


var host = builder.Build();

var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
var gameData = host.Services.GetRequiredService<GameData>();

await gameData.LoadAllDataAsync(httpClient);

await host.RunAsync();