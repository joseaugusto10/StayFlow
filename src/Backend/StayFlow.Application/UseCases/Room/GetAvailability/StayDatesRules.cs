using FluentValidation;
using StayFlow.Domain.Providers;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Room.GetAvailability;

public static class StayDatesRules
{
	public const int MAX_NIGHTS = 30;

	public static void AddStayDatesRules<T>(
		this AbstractValidator<T> validator,
		Func<T, DateOnly> checkIn,
		Func<T, DateOnly> checkOut,
		IDateProvider dateProvider)
	{
		validator.RuleFor(request => request)
			.Cascade(CascadeMode.Stop)
			.Must(request => checkOut(request) > checkIn(request))
			.OverridePropertyName("StayDates")
			.WithMessage(ResourceMessagesException.VALIDATION_DATES_INVALID)
			.Must(request => checkIn(request) >= dateProvider.Today)
			.WithMessage(ResourceMessagesException.VALIDATION_CHECKIN_IN_PAST)
			.Must(request => checkOut(request).DayNumber - checkIn(request).DayNumber <= MAX_NIGHTS)
			.WithMessage(ResourceMessagesException.VALIDATION_STAY_TOO_LONG);
	}
}
