using FluentValidation.Results;
using StayFlow.Application.Mappings;
using StayFlow.Application.UseCases.Shared;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Identity;
using StayFlow.Domain.Repositories;
using StayFlow.Domain.Repositories.Room;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Room.Register;

public class RegisterRoomUseCase : IRegisterRoomUseCase
{
	private readonly IRoomWriteOnlyRepository _roomWriteOnlyRepository;
	private readonly IRoomReadOnlyRepository _roomReadOnlyRepository;
	private readonly ILoggedUser _loggedUser;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterRoomUseCase(
		IRoomWriteOnlyRepository roomWriteOnlyRepository,
		IRoomReadOnlyRepository roomReadOnlyRepository,
		ILoggedUser loggedUser,
		IUnitOfWork unitOfWork)
	{
		_roomWriteOnlyRepository = roomWriteOnlyRepository;
		_roomReadOnlyRepository = roomReadOnlyRepository;
		_loggedUser = loggedUser;
		_unitOfWork = unitOfWork;
	}

	public async Task<ResponseRoomJson> Execute(RequestRoomJson request, CancellationToken cancellationToken = default)
	{
		var hotelId = _loggedUser.GetHotelId();

		var result = new RoomValidator().Validate(request);
		if (result.IsValid && await _roomReadOnlyRepository.ExistRoomWithNumber(hotelId, request.Number.Trim(), null, cancellationToken))
			result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_ROOM_NUMBER_ALREADY_EXISTS));
		result.ThrowIfInvalid();

		var room = RoomMapper.RequestToEntity(request, hotelId);

		await _roomWriteOnlyRepository.Add(room);
		await _unitOfWork.CommitOrThrowOnConflict(cancellationToken);

		return RoomMapper.ToResponse(room);
	}
}
