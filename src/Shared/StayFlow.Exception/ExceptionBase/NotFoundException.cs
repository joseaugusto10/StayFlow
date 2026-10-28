using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public sealed class NotFoundException : StayFlowException
{
	public NotFoundException(string message) : base(message) { }

	public override List<string> GetErrorMessages() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
