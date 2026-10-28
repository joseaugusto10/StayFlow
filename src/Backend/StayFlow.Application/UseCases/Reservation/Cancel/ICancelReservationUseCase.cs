namespace StayFlow.Application.UseCases.Reservation.Cancel;

public interface ICancelReservationUseCase
{
	Task Execute(Guid reservationId, CancellationToken cancellationToken = default);
}
