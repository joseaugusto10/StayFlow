using FluentValidation;
using StayFlow.Application.UseCases.Room.GetAvailability;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Providers;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Reservation.Create;

public class CreateReservationValidator : AbstractValidator<RequestCreateReservationJson>
{
	public CreateReservationValidator(IDateProvider dateProvider)
	{
		RuleFor(request => request.RoomId).NotEmpty().WithMessage(ResourceMessagesException.ROOM_NOT_FOUND);
		RuleFor(request => request.GuestId).NotEmpty().WithMessage(ResourceMessagesException.GUEST_NOT_FOUND);
		RuleFor(request => request.Adults).InclusiveBetween(1, 10).WithMessage(ResourceMessagesException.VALIDATION_ADULTS_INVALID);
		RuleFor(request => request.Children).InclusiveBetween(0, 10).WithMessage(ResourceMessagesException.VALIDATION_CHILDREN_INVALID);

		this.AddStayDatesRules(request => request.CheckInDate, request => request.CheckOutDate, dateProvider);
	}
}
