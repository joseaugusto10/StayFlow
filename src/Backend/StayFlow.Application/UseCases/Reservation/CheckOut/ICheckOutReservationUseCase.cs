namespace StayFlow.Application.UseCases.Reservation.CheckOut;

public interface ICheckOutReservationUseCase
{
	Task Execute(Guid reservationId, CancellationToken cancellationToken = default);
}
