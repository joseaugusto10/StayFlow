using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public sealed class ConflictException : StayFlowException
{
	public ConflictException(string message) : base(message) { }

	public override List<string> GetErrorMessages() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;
}
