using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using StayFlow.Api.Authorization;
using StayFlow.Api.Token;
using StayFlow.Communication.Responses;
using StayFlow.Domain.Repositories.User;
using StayFlow.Exception;

namespace StayFlow.Api.Extensions;

public static class AuthenticationExtensions
{
	public static void AddJwtCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(jwtOptions =>
			{
				var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

				jwtOptions.MapInboundClaims = false;
				jwtOptions.TokenValidationParameters = new()
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
					ValidateAudience = false,
					ValidateIssuer = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					NameClaimType = "name",
					RoleClaimType = "role"
				};

				jwtOptions.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						context.Token = context.Request.Cookies[HttpContextTokenProvider.COOKIE_NAME];
						if (string.IsNullOrWhiteSpace(context.Token))
							context.NoResult();

						return Task.CompletedTask;
					},

					OnTokenValidated = async context =>
					{
						var subject = context.Principal?.FindFirstValue("sub");

						if (Guid.TryParse(subject, out var userId) == false)
						{
							context.Fail("Invalid token subject.");
							return;
						}

						var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

						if (await userRepository.ExistActiveUserWithId(userId) == false)
							context.Fail("User not found or inactive.");
					},

					OnChallenge = async context =>
					{
						context.HandleResponse();

						context.Response.StatusCode = StatusCodes.Status401Unauthorized;

						var response = context.AuthenticateFailure switch
						{
							null => new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED),
							SecurityTokenExpiredException => new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED, accessTokenExpired: true),
							_ => new ResponseErrorJson(ResourceMessagesException.VALIDATION_RESOURCE_TOKEN_DENIED),
						};

						await context.Response.WriteAsJsonAsync(response);
					},

					OnForbidden = async context =>
					{
						context.Response.StatusCode = StatusCodes.Status403Forbidden;
						await context.Response.WriteAsJsonAsync(new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_FORBIDDEN));
					}
				};
			});

		services.AddAuthorizationBuilder()
			.AddPolicy(AuthorizationPolicies.MANAGER_ONLY, policy => policy.RequireRole("Manager"));
	}
}
