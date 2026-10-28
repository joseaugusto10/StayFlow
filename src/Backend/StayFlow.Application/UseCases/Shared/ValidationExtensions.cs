using FluentValidation.Results;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Application.UseCases.Shared;

internal static class ValidationExtensions
{
	public static void ThrowIfInvalid(this ValidationResult result)
	{
		if (result.IsValid)
			return;

		var errorMessages = result.Errors.Select(error => error.ErrorMessage).Distinct().ToList();
		throw new ErrorOnValidationException(errorMessages);
	}
}
