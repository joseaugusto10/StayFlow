using StayFlow.Domain.Entities;
using StayFlow.Domain.Services;
using StayFlow.UnitTests.Common;

namespace StayFlow.UnitTests.Domain;

public class HotelMetricsCalculatorTests
{
	private static readonly DateOnly Monday = FixedDateProvider.MONDAY;

	private static Reservation Stay(int offset, int nights, decimal total) => new()
	{
		CheckInDate = Monday.AddDays(offset),
		CheckOutDate = Monday.AddDays(offset + nights),
		TotalAmount = total
	};

	[Fact]
	public void Departure_Day_Is_Not_An_Occupied_Night()
	{
		var reservation = Stay(0, 2, 200m);

		HotelMetricsCalculator.OccupiesNight(reservation, Monday).ShouldBeTrue();
		HotelMetricsCalculator.OccupiesNight(reservation, Monday.AddDays(1)).ShouldBeTrue();
		HotelMetricsCalculator.OccupiesNight(reservation, Monday.AddDays(2)).ShouldBeFalse();
	}

	[Fact]
	public void Occupancy_Rate_Handles_Zero_Rooms()
	{
		HotelMetricsCalculator.OccupancyRate(3, 4).ShouldBe(75m);
		HotelMetricsCalculator.OccupancyRate(0, 0).ShouldBe(0m);
	}

	[Fact]
	public void Revenue_Is_Recognised_Only_For_Nights_Inside_The_Period()
	{
		// 4 nights at 100/night, but only the last 2 fall inside the period.
		var reservations = new[] { Stay(-2, 4, 400m) };

		var summary = HotelMetricsCalculator.Revenue(reservations, Monday, Monday.AddDays(10), totalRooms: 2);

		summary.RoomNightsSold.ShouldBe(2);
		summary.Revenue.ShouldBe(200m);
		summary.AverageDailyRate.ShouldBe(100m);
		summary.RevPar.ShouldBe(10m); // 200 / (2 rooms * 10 nights)
	}
}
