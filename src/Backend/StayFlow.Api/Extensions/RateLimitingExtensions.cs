using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using StayFlow.Api.Authorization;
using StayFlow.Communication.Responses;
using StayFlow.Exception;

namespace StayFlow.Api.Extensions;

public static class RateLimitingExtensions
{
	public static void AddAuthRateLimiting(this IServiceCollection services)
	{
		services.AddRateLimiter(options =>
		{
			options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

			options.AddPolicy(RateLimitPolicies.AUTH, httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 5,
						Window = TimeSpan.FromMinutes(1),
						QueueLimit = 0
					}));

			options.OnRejected = async (context, cancellationToken) =>
			{
				await context.HttpContext.Response.WriteAsJsonAsync(
					new ResponseErrorJson(ResourceMessagesException.TOO_MANY_REQUESTS), cancellationToken);
			};
		});
	}
}
