using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.Domain;

public class ReservationTests
{
	private static readonly DateOnly Today = FixedDateProvider.MONDAY;

	private static Reservation NewReservation(ReservationStatus status = ReservationStatus.Confirmed) => new()
	{
		CheckInDate = Today,
		CheckOutDate = Today.AddDays(3),
		Status = status
	};

	[Fact]
	public void Confirmed_Reservation_Can_Check_In_During_Stay()
	{
		NewReservation().CanCheckIn(Today).ShouldBeTrue();
		NewReservation().CanCheckIn(Today.AddDays(2)).ShouldBeTrue();
	}

	[Fact]
	public void Cannot_Check_In_Before_Arrival_Or_On_Departure_Day()
	{
		NewReservation().CanCheckIn(Today.AddDays(-1)).ShouldBeFalse();
		NewReservation().CanCheckIn(Today.AddDays(3)).ShouldBeFalse();
	}

	[Theory]
	[InlineData(ReservationStatus.CheckedIn)]
	[InlineData(ReservationStatus.CheckedOut)]
	[InlineData(ReservationStatus.Cancelled)]
	public void Only_Confirmed_Reservations_Can_Be_Cancelled(ReservationStatus status)
	{
		NewReservation(status).CanCancel().ShouldBeFalse();
	}

	[Fact]
	public void Full_Lifecycle_Sets_Status_And_Timestamps()
	{
		var reservation = NewReservation();

		reservation.CheckIn();
		reservation.Status.ShouldBe(ReservationStatus.CheckedIn);
		reservation.CheckedInAt.ShouldNotBeNull();
		reservation.CanCheckOut().ShouldBeTrue();

		reservation.CheckOut();
		reservation.Status.ShouldBe(ReservationStatus.CheckedOut);
		reservation.CheckedOutAt.ShouldNotBeNull();
		reservation.Nights.ShouldBe(3);
	}
}
