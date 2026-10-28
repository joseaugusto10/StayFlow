using FluentValidation;
using StayFlow.Communication.Requests;
using StayFlow.Domain.Providers;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Room.GetAvailability;

public class GetRoomAvailabilityValidator : AbstractValidator<RequestAvailabilityJson>
{
	public GetRoomAvailabilityValidator(IDateProvider dateProvider)
	{
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(request => request.Guests).InclusiveBetween(1, 10)
			.WithMessage(ResourceMessagesException.VALIDATION_ROOM_CAPACITY_INVALID);

		this.AddStayDatesRules(request => request.CheckIn, request => request.CheckOut, dateProvider);
	}
}
