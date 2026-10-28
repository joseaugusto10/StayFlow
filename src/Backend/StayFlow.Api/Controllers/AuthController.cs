using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using StayFlow.Api.Authorization;
using StayFlow.Api.Token;
using StayFlow.Application.UseCases.User.Login.WithEmailAndPassword;
using StayFlow.Communication.Requests;
using StayFlow.Communication.Responses;

namespace StayFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	[HttpPost]
	[EnableRateLimiting(RateLimitPolicies.AUTH)]
	[ProducesResponseType(typeof(ResponseLoggedInJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status429TooManyRequests)]
	public async Task<IActionResult> Login(
		[FromBody] RequestLoginJson request,
		[FromServices] ILoginWithEmailAndPasswordUseCase useCase)
	{
		var response = await useCase.Execute(request);

		var cookieOptions = BuildCookieOptions();
		if (request.Remember)
			cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(30);

		Response.Cookies.Append(HttpContextTokenProvider.COOKIE_NAME, response.Tokens.AccessToken, cookieOptions);

		return Ok(new ResponseLoggedInJson { Name = response.Name });
	}

	[HttpPost("logout")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public IActionResult Logout()
	{
		Response.Cookies.Delete(HttpContextTokenProvider.COOKIE_NAME, BuildCookieOptions());
		return NoContent();
	}

	private static CookieOptions BuildCookieOptions() => new()
	{
		HttpOnly = true,
		Secure = true,
		SameSite = SameSiteMode.None,
		Path = "/"
	};
}
