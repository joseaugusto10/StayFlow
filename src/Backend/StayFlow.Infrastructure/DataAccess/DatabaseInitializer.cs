using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Security.PasswordHashing;
using StayFlow.Domain.Services;

namespace StayFlow.Infrastructure.DataAccess;

public static class DatabaseInitializer
{
	public const string DEMO_PASSWORD = "Demo@1234";

	public static async Task InitializeAsync(IServiceProvider serviceProvider, bool seedDemoData)
	{
		await using var scope = serviceProvider.CreateAsyncScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<StayFlowDbContext>();

		await dbContext.Database.EnsureCreatedAsync();

		if (seedDemoData == false || await dbContext.Users.AnyAsync())
			return;

		var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
		var today = scope.ServiceProvider.GetRequiredService<IDateProvider>().Today;

		var hotel = new Hotel { Name = "Pousada Mar Azul", City = "Florianópolis", Stars = 4 };
		var passwordHash = passwordHasher.HashPassword(DEMO_PASSWORD);

		dbContext.Hotels.Add(hotel);
		dbContext.Users.AddRange(
			new User { Name = "Marina Costa", Email = "manager@stayflow.dev", PasswordHash = passwordHash, Role = UserRole.Manager, HotelId = hotel.Id },
			new User { Name = "Lucas Andrade", Email = "frontdesk@stayflow.dev", PasswordHash = passwordHash, Role = UserRole.Receptionist, HotelId = hotel.Id });

		var rooms = new List<Room>
		{
			NewRoom(hotel, "101", 1, RoomType.Single, 1, 220m, "Garden view"),
			NewRoom(hotel, "102", 1, RoomType.Double, 2, 320m, "Garden view, queen bed"),
			NewRoom(hotel, "103", 1, RoomType.Double, 2, 320m, "Accessible room"),
			NewRoom(hotel, "201", 2, RoomType.Double, 2, 380m, "Partial sea view"),
			NewRoom(hotel, "202", 2, RoomType.Family, 4, 520m, "Two bedrooms"),
			NewRoom(hotel, "203", 2, RoomType.Family, 5, 560m, "Balcony and bunk beds"),
			NewRoom(hotel, "301", 3, RoomType.Suite, 2, 890m, "Ocean front, hot tub"),
			NewRoom(hotel, "302", 3, RoomType.Suite, 3, 990m, "Presidential suite"),
		};
		rooms[2].Status = RoomStatus.Maintenance;
		dbContext.Rooms.AddRange(rooms);

		var guests = new List<Guest>
		{
			NewGuest(hotel, "Ana Souza", "52998224725", "ana.souza@mail.com", "+55 48 99911-0001", "Brazilian"),
			NewGuest(hotel, "John Miller", "X1234567", "john.miller@mail.com", "+1 415 555 0101", "American"),
			NewGuest(hotel, "Lucía Fernández", "AR3345678", "lucia.f@mail.com", "+54 11 5555 0102", "Argentine"),
			NewGuest(hotel, "Pedro Henrique Lima", "11144477735", "pedro.lima@mail.com", "+55 11 98888-0003", "Brazilian"),
			NewGuest(hotel, "Sophie Martin", "FR998877", "sophie.martin@mail.com", "+33 6 55 55 01 04", "French"),
			NewGuest(hotel, "Carlos Pereira", "39053344705", "carlos.p@mail.com", "+55 21 97777-0005", "Brazilian"),
		};
		dbContext.Guests.AddRange(guests);


		var plan = new (int Room, int Guest, int Offset, int Nights, ReservationStatus Status)[]
		{
			(0, 0, -3, 5, ReservationStatus.CheckedIn),
			(1, 1, -2, 2, ReservationStatus.CheckedIn),
			(3, 2, 0, 4, ReservationStatus.Confirmed),
			(4, 3, 0, 3, ReservationStatus.Confirmed),
			(6, 4, -1, 6, ReservationStatus.CheckedIn),
			(5, 5, 2, 7, ReservationStatus.Confirmed),
			(7, 1, 5, 3, ReservationStatus.Confirmed),
			(1, 3, 4, 2, ReservationStatus.Confirmed),
			(3, 0, 8, 4, ReservationStatus.Confirmed),
			(0, 5, -12, 4, ReservationStatus.CheckedOut),
			(6, 2, -9, 3, ReservationStatus.CheckedOut),
			(4, 4, -6, 2, ReservationStatus.Cancelled),
		};

		foreach (var item in plan)
		{
			var room = rooms[item.Room];
			var checkIn = today.AddDays(item.Offset);
			var checkOut = checkIn.AddDays(item.Nights);
			var quote = StayPricingCalculator.Calculate(room.NightlyRate, checkIn, checkOut);

			dbContext.Reservations.Add(new Reservation
			{
				Code = Reservation.GenerateCode(),
				HotelId = hotel.Id,
				RoomId = room.Id,
				GuestId = guests[item.Guest].Id,
				CheckInDate = checkIn,
				CheckOutDate = checkOut,
				Adults = Math.Min(2, room.Capacity),
				TotalAmount = quote.Total,
				Status = item.Status,
				CheckedInAt = item.Status is ReservationStatus.CheckedIn or ReservationStatus.CheckedOut ? DateTime.UtcNow.AddDays(item.Offset) : null,
				CheckedOutAt = item.Status == ReservationStatus.CheckedOut ? DateTime.UtcNow.AddDays(item.Offset + item.Nights) : null,
				CancelledAt = item.Status == ReservationStatus.Cancelled ? DateTime.UtcNow.AddDays(item.Offset - 1) : null
			});
		}

		await dbContext.SaveChangesAsync();
	}

	private static Room NewRoom(Hotel hotel, string number, int floor, RoomType type, int capacity, decimal rate, string description) =>
		new() { HotelId = hotel.Id, Number = number, Floor = floor, Type = type, Capacity = capacity, NightlyRate = rate, Description = description };

	private static Guest NewGuest(Hotel hotel, string name, string document, string email, string phone, string nationality) =>
		new() { HotelId = hotel.Id, Name = name, Document = document, Email = email, Phone = phone, Nationality = nationality };
}
