using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Reservation.GetById;

public interface IGetReservationByIdUseCase
{
	Task<ResponseReservationDetailsJson> Execute(Guid reservationId, CancellationToken cancellationToken = default);
}
