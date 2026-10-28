using Microsoft.AspNetCore.Localization;
using Scalar.AspNetCore;
using System.Globalization;
using System.Text.Json.Serialization;
using StayFlow.Api.Configurations;
using StayFlow.Api.Extensions;
using StayFlow.Api.Filters;
using StayFlow.Api.Middlewares;
using StayFlow.Api.Token;
using StayFlow.Application;
using StayFlow.Domain.Security.Tokens;
using StayFlow.Infrastructure;
using StayFlow.Infrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>())
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.ConfigureOptions<ApiBehaviorOptionsConfiguration>();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddScoped<IAccessTokenProvider, HttpContextTokenProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddJwtCookieAuthentication(builder.Configuration);
builder.Services.AddAuthRateLimiting();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR"), new("es") };

	options.DefaultRequestCulture = new RequestCulture("en");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
	options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("Frontend", policy =>
	{
		var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
		policy.WithOrigins(origins)
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app.Services, builder.Configuration.GetValue<bool>("SeedDemoData"));

app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
