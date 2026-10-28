using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public sealed class InvalidLoginException : StayFlowException
{
	public override List<string> GetErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
