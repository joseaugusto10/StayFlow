using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StayFlow.Communication.Responses;
using StayFlow.Exception;
using StayFlow.Exception.ExceptionBase;

namespace StayFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
	private readonly ILogger<ExceptionFilter> _logger;

	public ExceptionFilter(ILogger<ExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		if (context.Exception is OperationCanceledException &&
			context.HttpContext.RequestAborted.IsCancellationRequested)
		{
			context.ExceptionHandled = true;
			return;
		}

		if (context.Exception is StayFlowException stayFlowException)
		{
			context.HttpContext.Response.StatusCode = (int)stayFlowException.GetStatusCode();
			context.Result = new ObjectResult(new ResponseErrorJson(stayFlowException.GetErrorMessages()));
			return;
		}

		_logger.LogError(context.Exception, "Unhandled exception on {Path}", context.HttpContext.Request.Path);

		context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
	}
}
