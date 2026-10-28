using StayFlow.Communication.Responses;

namespace StayFlow.Application.UseCases.Room.GetAll;

public interface IGetRoomsUseCase
{
	Task<List<ResponseRoomJson>> Execute(CancellationToken cancellationToken = default);
}
