using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Reservation.GetWithFilters;

public interface IGetReservationsWithFiltersUseCase
{
	Task<ResponseReservationsJson> Execute(RequestFiltersReservationsJson request, CancellationToken cancellationToken = default);
}
