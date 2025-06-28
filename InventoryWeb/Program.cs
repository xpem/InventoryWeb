using ApiRepos;
using InventoryWeb;
using InventoryWeb.Infra;
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
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<CustomAuthenticationService>();
builder.Services.AddSingleton<ToastService>();

builder.Services.AddTransient<IUserApiRepo, UserApiRepo>();

builder.Services.AddTransient<IUserService, UserService>();


await builder.Build().RunAsync();
