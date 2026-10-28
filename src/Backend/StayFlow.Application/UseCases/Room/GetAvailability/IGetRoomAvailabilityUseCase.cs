using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Room.GetAvailability;

public interface IGetRoomAvailabilityUseCase
{
	Task<List<ResponseAvailableRoomJson>> Execute(RequestAvailabilityJson request, CancellationToken cancellationToken = default);
}
