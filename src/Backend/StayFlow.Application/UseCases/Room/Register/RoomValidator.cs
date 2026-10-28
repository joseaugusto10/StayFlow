using FluentValidation;
using StayFlow.Communication.Requests;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Room.Register;

public class RoomValidator : AbstractValidator<RequestRoomJson>
{
	public RoomValidator()
	{
		RuleFor(room => room.Number).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_ROOM_NUMBER_REQUIRED);
		RuleFor(room => room.Type).IsInEnum().WithMessage(ResourceMessagesException.VALIDATION_ROOM_TYPE_INVALID);
		RuleFor(room => room.Capacity).InclusiveBetween(1, 10).WithMessage(ResourceMessagesException.VALIDATION_ROOM_CAPACITY_INVALID);
		RuleFor(room => room.NightlyRate).GreaterThan(0).WithMessage(ResourceMessagesException.VALIDATION_ROOM_RATE_INVALID);
	}
}
