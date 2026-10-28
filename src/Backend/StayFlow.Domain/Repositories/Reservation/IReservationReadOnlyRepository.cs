using StayFlow.Domain.Dtos;

namespace StayFlow.Domain.Repositories.Reservation;

public interface IReservationReadOnlyRepository
{
	Task<ReservationsReadResultDto> GetWithFilters(Guid hotelId, ReservationFilters filters, CancellationToken cancellationToken = default);
	Task<Entities.Reservation?> GetById(Guid hotelId, Guid reservationId, CancellationToken cancellationToken = default);
	Task<bool> HasOverlap(Guid roomId, DateOnly checkIn, DateOnly checkOut, CancellationToken cancellationToken = default);

	/// <summary>Non-cancelled reservations that intersect the [from, to) interval.</summary>
	Task<IList<Entities.Reservation>> GetActiveInRange(Guid hotelId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
}
