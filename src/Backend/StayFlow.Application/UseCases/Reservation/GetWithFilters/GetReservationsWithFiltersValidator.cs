using FluentValidation;
using StayFlow.Communication.Requests;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Reservation.GetWithFilters;

public class GetReservationsWithFiltersValidator : AbstractValidator<RequestFiltersReservationsJson>
{
	public GetReservationsWithFiltersValidator()
	{
		RuleFor(request => request.Page).GreaterThan(0).WithMessage(ResourceMessagesException.VALIDATION_PAGE_INVALID);
		RuleFor(request => request.PageSize).InclusiveBetween(1, 100).WithMessage(ResourceMessagesException.VALIDATION_PAGE_SIZE_INVALID);
		RuleFor(request => request.Status).IsInEnum().WithMessage(ResourceMessagesException.VALIDATION_RESERVATION_STATUS_INVALID)
			.When(request => request.Status.HasValue);
		RuleFor(request => request)
			.Must(request => request.From is null || request.To is null || request.From <= request.To)
			.OverridePropertyName("Period")
			.WithMessage(ResourceMessagesException.VALIDATION_DATES_INVALID);
	}
}
