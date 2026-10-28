using FluentValidation;
using StayFlow.Application.UseCases.Shared.Validators;
using StayFlow.Communication.Requests;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Guest.Register;

public class RegisterGuestValidator : AbstractValidator<RequestRegisterGuestJson>
{
	public RegisterGuestValidator()
	{
		RuleFor(guest => guest.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
		RuleFor(guest => guest.Document).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_GUEST_DOCUMENT_REQUIRED);
		RuleFor(guest => guest.Email).ValidEmail();
		RuleFor(guest => guest.Phone).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PHONE_REQUIRED);
	}
}
