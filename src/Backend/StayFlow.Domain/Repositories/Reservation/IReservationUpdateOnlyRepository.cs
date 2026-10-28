namespace StayFlow.Domain.Repositories.Reservation;

public interface IReservationUpdateOnlyRepository
{
	Task<Entities.Reservation?> GetById(Guid hotelId, Guid reservationId, CancellationToken cancellationToken = default);
}
