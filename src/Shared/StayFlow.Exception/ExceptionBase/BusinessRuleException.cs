using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public sealed class BusinessRuleException : StayFlowException
{
	public BusinessRuleException(string message) : base(message) { }

	public override List<string> GetErrorMessages() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.UnprocessableEntity;
}
