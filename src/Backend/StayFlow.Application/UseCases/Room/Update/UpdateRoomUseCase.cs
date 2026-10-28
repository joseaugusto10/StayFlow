using FluentValidation.Results;
using StayFlow.Application.Mappings;
using StayFlow.Application.UseCases.Room.Register;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Room.Update;

public class UpdateRoomUseCase : IUpdateRoomUseCase
{
	private readonly IRoomUpdateOnlyRepository _roomUpdateOnlyRepository;
	private readonly IRoomReadOnlyRepository _roomReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateRoomUseCase(
		IRoomUpdateOnlyRepository roomUpdateOnlyRepository,
		IRoomReadOnlyRepository roomReadOnlyRepository,
		ILoggedUser loggedUser,
		IUnitOfWork unitOfWork)
	{
		_roomUpdateOnlyRepository = roomUpdateOnlyRepository;
		_roomReadOnlyRepository = roomReadOnlyRepository;
		_loggedUser = loggedUser;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(Guid roomId, RequestRoomJson request, CancellationToken cancellationToken = default)
	{
		var hotelId = _loggedUser.GetHotelId();

		var room = await _roomUpdateOnlyRepository.GetById(hotelId, roomId, cancellationToken)
			?? throw new NotFoundException(ResourceMessagesException.ROOM_NOT_FOUND);

		var result = new RoomValidator().Validate(request);
		if (result.IsValid && await _roomReadOnlyRepository.ExistRoomWithNumber(hotelId, request.Number.Trim(), roomId, cancellationToken))
			result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_ROOM_NUMBER_ALREADY_EXISTS));
		result.ThrowIfInvalid();

		RoomMapper.ApplyRequest(room, request);

		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);
	}
}
