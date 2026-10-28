using FluentValidation;
using StayFlow.Application.UseCases.Shared.Validators;
using StayFlow.Communication.Requests;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
	public RegisterUserAccountValidator()
	{
		RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
		RuleFor(user => user.Email).ValidEmail();
		RuleFor(user => user.Password).Password();
		RuleFor(user => user.HotelName).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_HOTEL_NAME_REQUIRED);
		RuleFor(user => user.City).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_CITY_REQUIRED);
	}
}
