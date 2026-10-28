using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Reservation.Create;

public interface ICreateReservationUseCase
{
	Task<ResponseCreatedReservationJson> Execute(RequestCreateReservationJson request, CancellationToken cancellationToken = default);
}
