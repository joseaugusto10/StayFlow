using StayFlow.Communication.Requests;

namespace StayFlow.Application.UseCases.Room.Update;

public interface IUpdateRoomUseCase
{
	Task Execute(Guid roomId, RequestRoomJson request, CancellationToken cancellationToken = default);
}
