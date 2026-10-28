using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StayFlow.Communication.Responses;
using StayFlow.Exception;

namespace StayFlow.Api.Configurations;

public sealed class ApiBehaviorOptionsConfiguration : IConfigureOptions<ApiBehaviorOptions>
{
	public void Configure(ApiBehaviorOptions options)
	{
		options.InvalidModelStateResponseFactory = _ =>
			new BadRequestObjectResult(new ResponseErrorJson(ResourceMessagesException.VALIDATION_REQUEST_INVALID));
	}
}
