using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Providers;
using StayFlow.Domain.Repositories.Reservation;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Domain.Services;

namespace StayFlow.Application.UseCases.Dashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
	private const int FORECAST_DAYS = 14;

	private readonly IRoomReadOnlyRepository _roomReadOnlyRepository;
	private readonly IReservationReadOnlyRepository _reservationReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IDateProvider _dateProvider;

	public GetDashboardUseCase(
		IRoomReadOnlyRepository roomReadOnlyRepository,
		IReservationReadOnlyRepository reservationReadOnlyRepository,
		ILoggedUser loggedUser,
		IDateProvider dateProvider)
	{
		_roomReadOnlyRepository = roomReadOnlyRepository;
		_reservationReadOnlyRepository = reservationReadOnlyRepository;
		_loggedUser = loggedUser;
		_dateProvider = dateProvider;
	}

	public async Task<ResponseDashboardJson> Execute(CancellationToken cancellationToken = default)
	{
		var hotelId = _loggedUser.GetHotelId();
		var today = _dateProvider.Today;
		var monthStart = new DateOnly(today.Year, today.Month, 1);
		var monthEnd = monthStart.AddMonths(1);
		var forecastEnd = today.AddDays(FORECAST_DAYS);

		var totalRooms = await _roomReadOnlyRepository.CountActive(hotelId, cancellationToken);

		var rangeStart = Earliest(monthStart, today.AddDays(-1));
		var rangeEnd = Latest(monthEnd, forecastEnd);
		var reservations = await _reservationReadOnlyRepository.GetActiveInRange(hotelId, rangeStart, rangeEnd, cancellationToken);

		var occupiedToday = HotelMetricsCalculator.OccupiedRooms(reservations, today);
		var month = HotelMetricsCalculator.Revenue(reservations, monthStart, monthEnd, totalRooms);

		return new ResponseDashboardJson
		{
			TotalRooms = totalRooms,
			OccupiedToday = occupiedToday,
			OccupancyRateToday = HotelMetricsCalculator.OccupancyRate(occupiedToday, totalRooms),
			GuestsInHouse = reservations.Where(r => r.Status == ReservationStatus.CheckedIn).Sum(r => r.TotalGuests),
			RevenueThisMonth = month.Revenue,
			AverageDailyRate = month.AverageDailyRate,
			RevPar = month.RevPar,
			ArrivalsToday = reservations
				.Where(r => r.CheckInDate == today && r.Status == ReservationStatus.Confirmed)
				.Select(ReservationMapper.ToListItem).ToList(),
			DeparturesToday = reservations
				.Where(r => r.CheckOutDate == today && r.Status == ReservationStatus.CheckedIn)
				.Select(ReservationMapper.ToListItem).ToList(),
			OccupancyForecast = Enumerable.Range(0, FORECAST_DAYS)
				.Select(offset =>
				{
					var date = today.AddDays(offset);
					var occupied = HotelMetricsCalculator.OccupiedRooms(reservations, date);
					return new ResponseOccupancyPointJson
					{
						Date = date,
						OccupiedRooms = occupied,
						Rate = HotelMetricsCalculator.OccupancyRate(occupied, totalRooms)
					};
				})
				.ToList()
		};
	}

	private static DateOnly Earliest(DateOnly a, DateOnly b) => a < b ? a : b;
	private static DateOnly Latest(DateOnly a, DateOnly b) => a > b ? a : b;
}
