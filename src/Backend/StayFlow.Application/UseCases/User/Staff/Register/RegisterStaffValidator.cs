using FluentValidation;
using StayFlow.Application.UseCases.Shared.Validators;
using StayFlow.Communication.Requests;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.User.Staff.Register;

public class RegisterStaffValidator : AbstractValidator<RequestRegisterStaffJson>
{
	public RegisterStaffValidator()
	{
		RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
		RuleFor(user => user.Email).ValidEmail();
		RuleFor(user => user.Password).Password();
	}
}
