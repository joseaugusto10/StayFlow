using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;
using DomainEnums = StayFlow.Domain.Enums;

namespace StayFlow.Application.UseCases.Room.ChangeStatus;

public class ChangeRoomStatusUseCase : IChangeRoomStatusUseCase
{
	private readonly IRoomUpdateOnlyRepository _roomUpdateOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IUnitOfWork _unitOfWork;

	public ChangeRoomStatusUseCase(
		IRoomUpdateOnlyRepository roomUpdateOnlyRepository,
		ILoggedUser loggedUser,
		IUnitOfWork unitOfWork)
	{
		_roomUpdateOnlyRepository = roomUpdateOnlyRepository;
		_loggedUser = loggedUser;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(Guid roomId, RequestChangeRoomStatusJson request, CancellationToken cancellationToken = default)
	{
		if (Enum.IsDefined(request.Status) == false)
			throw new ErrorOnValidationException([ResourceMessagesException.VALIDATION_ROOM_STATUS_INVALID]);

		var room = await _roomUpdateOnlyRepository.GetById(_loggedUser.GetHotelId(), roomId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.ROOM_NOT_FOUND);

		room.Status = (DomainEnums.RoomStatus)request.Status;
		room.Touch();

		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);
	}
}
