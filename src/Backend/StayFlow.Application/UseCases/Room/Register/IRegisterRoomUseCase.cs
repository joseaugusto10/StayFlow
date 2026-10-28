using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Room.Register;

public interface IRegisterRoomUseCase
{
	Task<ResponseRoomJson> Execute(RequestRoomJson request, CancellationToken cancellationToken = default);
}
