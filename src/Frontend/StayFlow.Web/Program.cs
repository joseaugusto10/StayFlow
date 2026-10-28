using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StayFlow.Web;
using StayFlow.Web.Auth;
using StayFlow.Web.Services;
using StayFlow.Web.Services.Localization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7001/";

builder.Services.AddTransient<CookieHandler>();
builder.Services
	.AddHttpClient(ApiClient.HTTP_CLIENT_NAME, client => client.BaseAddress = new Uri(apiBaseUrl))
	.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<I18n>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<ToastService>();

builder.Services.AddAuthorizationCore(options =>
	options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager")));
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<StayFlowAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<StayFlowAuthStateProvider>());

var host = builder.Build();

await host.Services.GetRequiredService<I18n>().InitializeAsync();

await host.RunAsync();
