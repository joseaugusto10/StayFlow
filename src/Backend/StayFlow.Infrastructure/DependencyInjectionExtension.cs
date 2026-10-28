using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Guest;
using StayFlow.Domain.Repositories.Hotel;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Domain.Repositories.User;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Domain.Security.Tokens;
using StayFlow.Infrastructure.DataAccess;
using StayFlow.Infrastructure.DataAccess.Repositories;
using StayFlow.Infrastructure.Identity;
using StayFlow.Infrastructure.Providers;
using StayFlow.Infrastructure.Security.PasswordHashing;
using StayFlow.Infrastructure.Tokens.Access;

namespace StayFlow.Infrastructure;

public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddTokensHandlers(services, configuration);
		AddDatabase(services, configuration);
		AddRepositories(services);

		services.AddScoped<ILoggedUser, LoggedUser>();
		services.AddSingleton<IDateProvider>(_ =>
			new HotelDateProvider(configuration.GetValue<string>("Hotel:TimeZone") ?? "America/Sao_Paulo"));
	}

	private static void AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
		services.AddScoped<IUserReadOnlyRepository, UserRepository>();

		services.AddScoped<IHotelWriteOnlyRepository, HotelRepository>();

		services.AddScoped<IRoomWriteOnlyRepository, RoomRepository>();
		services.AddScoped<IRoomReadOnlyRepository, RoomRepository>();
		services.AddScoped<IRoomUpdateOnlyRepository, RoomRepository>();

		services.AddScoped<IGuestWriteOnlyRepository, GuestRepository>();
		services.AddScoped<IGuestReadOnlyRepository, GuestRepository>();

		services.AddScoped<IReservationWriteOnlyRepository, ReservationRepository>();
		services.AddScoped<IReservationReadOnlyRepository, ReservationRepository>();
		services.AddScoped<IReservationUpdateOnlyRepository, ReservationRepository>();
	}

	private static void AddTokensHandlers(IServiceCollection services, IConfiguration configuration)
	{
		var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes");
		var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

		services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
		services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenHandler(expirationTimeMinutes, signingKey));
	}

	private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<StayFlowDbContext>(config =>
		{
			var connectionString = configuration.GetConnectionString("DbConnection")!;
			config.UseSqlite(connectionString);
		});

		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
