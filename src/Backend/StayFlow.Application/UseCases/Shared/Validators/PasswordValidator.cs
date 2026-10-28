using FluentValidation;
using StayFlow.Exception;

namespace StayFlow.Application.UseCases.Shared.Validators;

public static class PasswordValidator
{
	internal static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
	{
		return ruleBuilder
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)
				.MinimumLength(8)
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH)
				.Matches("[A-Z]")
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_UPPERCASE_REQUIRED)
				.Matches("[a-z]")
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_LOWERCASE_REQUIRED)
				.Matches("[0-9]")
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_NUMBER_REQUIRED)
				.Matches(@"[^A-Za-z0-9\s]")
				.WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_SPECIAL_CHARACTER_REQUIRED);
	}

	internal static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
	{
		return ruleBuilder
				.Cascade(CascadeMode.Stop)
				.NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED)
				.MaximumLength(254).WithMessage(ResourceMessagesException.VALIDATION_EMAIL_MAX_LENGTH)
				.EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
	}
}
