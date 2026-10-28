using StayFlow.Communication.Requests;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;

namespace StayFlow.UnitTests.Common;

public static class Builders
{
	public static readonly Guid HotelId = Guid.Parse("11111111-1111-1111-1111-111111111111");

	public static Room Room(int capacity = 2, decimal rate = 100m, RoomStatus status = RoomStatus.Available) => new()
	{
		HotelId = HotelId,
		Number = "101",
		Capacity = capacity,
		NightlyRate = rate,
		Status = status,
		Type = RoomType.Double
	};

	public static Guest Guest() => new()
	{
		HotelId = HotelId,
		Name = "Ana Souza",
		Document = "52998224725",
		Email = "ana@mail.com",
		Phone = "+55 48 99999-0000"
	};

	public static RequestCreateReservationJson CreateReservation(Guid roomId, Guid guestId, int nights = 2, int adults = 2) => new()
	{
		RoomId = roomId,
		GuestId = guestId,
		CheckInDate = FixedDateProvider.MONDAY,
		CheckOutDate = FixedDateProvider.MONDAY.AddDays(nights),
		Adults = adults
	};

	public static RequestRegisterUserAccountJson RegisterUser() => new()
	{
		Name = "Marina Costa",
		Email = "marina@hotel.com",
		Password = "Str0ng@Pass",
		HotelName = "Pousada Mar Azul",
		City = "Florianópolis"
	};
}
