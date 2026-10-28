using StayFlow.Communication.Requests;

namespace StayFlow.Application.UseCases.Room.ChangeStatus;

public interface IChangeRoomStatusUseCase
{
	Task Execute(Guid roomId, RequestChangeRoomStatusJson request, CancellationToken cancellationToken = default);
}
