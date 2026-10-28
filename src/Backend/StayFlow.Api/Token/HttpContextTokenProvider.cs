using StayFlow.Domain.Security.Tokens;

namespace StayFlow.Api.Token;

public sealed class HttpContextTokenProvider : IAccessTokenProvider
{
	public const string COOKIE_NAME = "accessToken";

	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string GetToken()
	{
		var token = _httpContextAccessor.HttpContext?.Request.Cookies[COOKIE_NAME];

		if (string.IsNullOrWhiteSpace(token))
			throw new InvalidOperationException("Access token not found.");

		return token;
	}
}
