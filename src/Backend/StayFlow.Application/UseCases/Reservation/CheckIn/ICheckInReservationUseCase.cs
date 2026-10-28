namespace StayFlow.Application.UseCases.Reservation.CheckIn;

public interface ICheckInReservationUseCase
{
	Task Execute(Guid reservationId, CancellationToken cancellationToken = default);
}
