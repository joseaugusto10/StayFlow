using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public sealed class ErrorOnValidationException : StayFlowException
{
	private readonly List<string> _errors;

	public ErrorOnValidationException(List<string> errorMessages)
	{
		_errors = errorMessages;
	}

	public override List<string> GetErrorMessages() => _errors;

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
