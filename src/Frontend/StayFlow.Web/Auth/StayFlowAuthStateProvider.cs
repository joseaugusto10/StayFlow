using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using StayFlow.Communication.Responses;
using StayFlow.Web.Services;

namespace StayFlow.Web.Auth;

public sealed class StayFlowAuthStateProvider : AuthenticationStateProvider
{
	private static readonly AuthenticationState Anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

	private readonly IHttpClientFactory _httpClientFactory;
	private Task<AuthenticationState>? _cached;

	public ResponseUserProfileJson? CurrentUser { get; private set; }

	public StayFlowAuthStateProvider(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync() => _cached ??= LoadAsync();

	public void Refresh()
	{
		_cached = null;
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public void SignOut()
	{
		CurrentUser = null;
		_cached = Task.FromResult(Anonymous);
		NotifyAuthenticationStateChanged(_cached);
	}

	private async Task<AuthenticationState> LoadAsync()
	{
		try
		{
			var client = _httpClientFactory.CreateClient(ApiClient.HTTP_CLIENT_NAME);
			var response = await client.GetAsync("api/users/profile");

			if (response.IsSuccessStatusCode == false)
				return Anonymous;

			CurrentUser = await response.Content.ReadFromJsonAsync<ResponseUserProfileJson>(ApiClient.JsonOptions);
			if (CurrentUser is null)
				return Anonymous;

			var identity = new ClaimsIdentity(
			[
				new Claim(ClaimTypes.NameIdentifier, CurrentUser.Id.ToString()),
				new Claim(ClaimTypes.Name, CurrentUser.Name),
				new Claim(ClaimTypes.Email, CurrentUser.Email),
				new Claim(ClaimTypes.Role, CurrentUser.Role.ToString()),
				new Claim("hotel", CurrentUser.HotelName)
			], authenticationType: "StayFlowCookie");

			return new AuthenticationState(new ClaimsPrincipal(identity));
		}
		catch (HttpRequestException)
		{
			return Anonymous;
		}
	}
}
