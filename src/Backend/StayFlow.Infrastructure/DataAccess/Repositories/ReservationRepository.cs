using Microsoft.EntityFrameworkCore;
using StayFlow.Domain.Dtos;
using StayFlow.Domain.Entities;
using StayFlow.Domain.Enums;
using StayFlow.Domain.Repositories.Reservation;

namespace StayFlow.Infrastructure.DataAccess.Repositories;

internal sealed class ReservationRepository :
	IReservationWriteOnlyRepository,
	IReservationReadOnlyRepository,
	IReservationUpdateOnlyRepository
{
	private readonly StayFlowDbContext _dbContext;

	public ReservationRepository(StayFlowDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Add(Reservation reservation) => await _dbContext.Reservations.AddAsync(reservation);

	public async Task<ReservationsReadResultDto> GetWithFilters(Guid hotelId, ReservationFilters filters, CancellationToken cancellationToken = default)
	{
		var query = _dbContext.Reservations
			.AsNoTracking()
			.Where(reservation => reservation.HotelId == hotelId);

		if (string.IsNullOrWhiteSpace(filters.Search) == false)
		{
			var pattern = $"%{filters.Search}%";
			query = query.Where(reservation =>
				EF.Functions.Like(reservation.Code, pattern) ||
				EF.Functions.Like(reservation.Guest.Name, pattern) ||
				EF.Functions.Like(reservation.Room.Number, pattern));
		}

		if (filters.Status.HasValue)
			query = query.Where(reservation => reservation.Status == filters.Status.Value);

		// Period filter: any reservation whose stay intersects [From, To].
		if (filters.From.HasValue)
			query = query.Where(reservation => reservation.CheckOutDate > filters.From.Value);

		if (filters.To.HasValue)
			query = query.Where(reservation => reservation.CheckInDate <= filters.To.Value);

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.Include(reservation => reservation.Guest)
			.Include(reservation => reservation.Room)
			.OrderByDescending(reservation => reservation.CheckInDate)
			.ThenBy(reservation => reservation.Code)
			.Skip((filters.Page - 1) * filters.PageSize)
			.Take(filters.PageSize)
			.ToListAsync(cancellationToken);

		return new ReservationsReadResultDto { Items = items, TotalCount = totalCount };
	}

	async Task<Reservation?> IReservationReadOnlyRepository.GetById(Guid hotelId, Guid reservationId, CancellationToken cancellationToken) =>
		await _dbContext.Reservations
			.AsNoTracking()
			.Include(reservation => reservation.Guest)
			.Include(reservation => reservation.Room)
			.FirstOrDefaultAsync(reservation => reservation.HotelId == hotelId && reservation.Id == reservationId, cancellationToken);

	async Task<Reservation?> IReservationUpdateOnlyRepository.GetById(Guid hotelId, Guid reservationId, CancellationToken cancellationToken) =>
		await _dbContext.Reservations
			.FirstOrDefaultAsync(reservation => reservation.HotelId == hotelId && reservation.Id == reservationId, cancellationToken);

	public async Task<bool> HasOverlap(Guid roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken cancellationToken = default) =>
		await _dbContext.Reservations.AnyAsync(reservation =>
			reservation.RoomId == roomId &&
			reservation.Status != ReservationStatus.Cancelled &&
			reservation.CheckInDate < checkOut &&
			checkIn < reservation.CheckOutDate, cancellationToken);

	public async Task<IList<Reservation>> GetActiveInRange(Guid hotelId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default) =>
		await _dbContext.Reservations
			.AsNoTracking()
			.Include(reservation => reservation.Guest)
			.Include(reservation => reservation.Room)
			.Where(reservation =>
				reservation.HotelId == hotelId &&
				reservation.Status != ReservationStatus.Cancelled &&
				reservation.CheckInDate < to &&
				from < reservation.CheckOutDate)
			.ToListAsync(cancellationToken);
}
