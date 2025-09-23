using ApiRepos;
using Blazored.LocalStorage;
using InventoryWeb;
using InventoryWeb.Infra;
using InventoryWeb.Infra.Handlers;
using InventoryWeb.Infra.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
//builder.Services.AddCascadingAuthenticationState();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<CustomAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

//front services
builder.Services.AddSingleton<ToastService>();
builder.Services.AddSingleton<NavMenuService>();


builder.Services.AddScoped<IHttpClientFunctions, HttpClientFunctions>();
builder.Services.AddScoped<IHttpClientWithFileFunctions, HttpClientWithFileFunctions>();

builder.Services.AddScoped<IUserApiRepo, UserApiRepo>();
builder.Services.AddScoped<ICategoryApiRepo, CategoryApiRepo>();
builder.Services.AddScoped<ISubCategoryApiRepo, SubCategoryApiRepo>();
builder.Services.AddScoped<IItemSituationApiRepo, ItemSituationApiRepo>();
builder.Services.AddScoped<IAcquisitionTypeApiRepo, AcquisitionTypeApiRepo>();
builder.Services.AddScoped<IItemApiRepo, ItemApiRepo>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IItemSituationService, ItemSituationService>();
builder.Services.AddScoped<IAcquisitionTypeService, AcquisitionTypeService>();
builder.Services.AddScoped<IItemService, ItemService>();

await builder.Build().RunAsync();
