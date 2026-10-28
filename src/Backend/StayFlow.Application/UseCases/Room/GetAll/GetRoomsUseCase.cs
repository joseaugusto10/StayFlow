using StayFlow.Application.Mappings;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories.Room;

namespace StayFlow.Application.UseCases.Room.GetAll;

public class GetRoomsUseCase : IGetRoomsUseCase
{
	private readonly IRoomReadOnlyRepository _roomReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;

	public GetRoomsUseCase(IRoomReadOnlyRepository roomReadOnlyRepository, ILoggedUser loggedUser)
	{
		_roomReadOnlyRepository = roomReadOnlyRepository;
		_loggedUser = loggedUser;
	}

	public async Task<List<ResponseRoomJson>> Execute(CancellationToken cancellationToken = default)
	{
		var rooms = await _roomReadOnlyRepository.GetAll(_loggedUser.GetHotelId(), cancellationToken);
		return rooms.Select(RoomMapper.ToResponse).ToList();
	}
}
