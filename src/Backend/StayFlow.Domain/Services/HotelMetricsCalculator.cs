using StayFlow.Domain.Entities;

namespace StayFlow.Domain.Services;

public static class HotelMetricsCalculator
{
	public static bool OccupiesNight(Reservation reservation, DateOnly night) =>
		reservation.CheckInDate <= night && night < reservation.CheckOutDate;

	public static int OccupiedRooms(IEnumerable<Reservation> reservations, DateOnly night) =>
		reservations.Count(reservation => OccupiesNight(reservation, night));

	public static decimal OccupancyRate(int occupiedRooms, int totalRooms) =>
		totalRooms <= 0 ? 0 : Math.Round(occupiedRooms * 100m / totalRooms, 1);

	public static RevenueSummary Revenue(IEnumerable<Reservation> reservations, DateOnly from, DateOnly to, int totalRooms)
	{
		var roomNightsSold = 0;
		var revenue = 0m;

		foreach (var reservation in reservations)
		{
			if (reservation.Nights <= 0)
				continue;

			var nightlyAverage = reservation.TotalAmount / reservation.Nights;
			var start = reservation.CheckInDate > from ? reservation.CheckInDate : from;
			var end = reservation.CheckOutDate < to ? reservation.CheckOutDate : to;

			var nightsInPeriod = Math.Max(0, end.DayNumber - start.DayNumber);
			roomNightsSold += nightsInPeriod;
			revenue += nightlyAverage * nightsInPeriod;
		}

		var roomNightsAvailable = Math.Max(0, totalRooms * (to.DayNumber - from.DayNumber));

		return new RevenueSummary(
			Revenue: Math.Round(revenue, 2),
			RoomNightsSold: roomNightsSold,
			AverageDailyRate: roomNightsSold == 0 ? 0 : Math.Round(revenue / roomNightsSold, 2),
			RevPar: roomNightsAvailable == 0 ? 0 : Math.Round(revenue / roomNightsAvailable, 2));
	}
}

public sealed record RevenueSummary(decimal Revenue, int RoomNightsSold, decimal AverageDailyRate, decimal RevPar);
