using System.Net;

namespace StayFlow.Exception.ExceptionBase;

public abstract class StayFlowException : System.Exception
{
	protected StayFlowException() { }
	protected StayFlowException(string message) : base(message) { }

	public abstract HttpStatusCode GetStatusCode();
	public abstract List<string> GetErrorMessages();
}
