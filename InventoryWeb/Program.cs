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

#region teste
// Configura o HttpClient para se comunicar com sua API de backend
// Usa HttpClientFactory para gerenciar instâncias de HttpClient
//builder.Services.AddHttpClient("API", client =>
//{
//    // URL base da sua API de backend (ajuste conforme necessário)
//    // Ex: "https://localhost:7001/" se sua API rodar localmente na porta 7001
//    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); // Ou a URL explícita da sua API
//})
//// Adiciona o handler para injetar o token JWT nas requisições
//.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

//// Registra o HttpClient padrão para ser injetado diretamente em componentes/serviços
//// Ele usará a configuração "API" definida acima
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

#endregion

builder.Services.AddSingleton<ToastService>();

builder.Services.AddTransient<IUserApiRepo, UserApiRepo>();

builder.Services.AddTransient<IUserService, UserService>();


await builder.Build().RunAsync();
