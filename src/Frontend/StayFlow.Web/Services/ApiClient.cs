using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using StayFlow.Communication.Responses;
using StayFlow.Web.Auth;
using StayFlow.Web.Services.Localization;

namespace StayFlow.Web.Services;

public sealed class ApiException : Exception
{
	public HttpStatusCode StatusCode { get; }
	public IReadOnlyList<string> Errors { get; }

	public ApiException(HttpStatusCode statusCode, IReadOnlyList<string> errors)
		: base(string.Join(" ", errors))
	{
		StatusCode = statusCode;
		Errors = errors;
	}
}

public sealed class ApiClient
{
	public const string HTTP_CLIENT_NAME = "StayFlowApi";

	public static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
	{
		Converters = { new JsonStringEnumConverter() }
	};

	private readonly HttpClient _http;
	private readonly StayFlowAuthStateProvider _authState;
	private readonly I18n _i18n;

	public ApiClient(IHttpClientFactory factory, StayFlowAuthStateProvider authState, I18n i18n)
	{
		_http = factory.CreateClient(HTTP_CLIENT_NAME);
		_authState = authState;
		_i18n = i18n;
	}

	public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default)
	{
		var response = await _http.GetAsync(url, cancellationToken);
		return await ReadAsync<T>(response, cancellationToken);
	}

	public async Task<T> PostAsync<T>(string url, object body, CancellationToken cancellationToken = default)
	{
		var response = await _http.PostAsJsonAsync(url, body, JsonOptions, cancellationToken);
		return await ReadAsync<T>(response, cancellationToken);
	}

	public async Task PostAsync(string url, object? body = null, CancellationToken cancellationToken = default)
	{
		var response = await _http.PostAsJsonAsync(url, body ?? new { }, JsonOptions, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	public async Task PutAsync(string url, object body, CancellationToken cancellationToken = default)
	{
		var response = await _http.PutAsJsonAsync(url, body, JsonOptions, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	public async Task PatchAsync(string url, object? body = null, CancellationToken cancellationToken = default)
	{
		var content = JsonContent.Create(body ?? new { }, options: JsonOptions);
		var response = await _http.PatchAsync(url, content, cancellationToken);
		await EnsureSuccessAsync(response, cancellationToken);
	}

	private async Task<T> ReadAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		await EnsureSuccessAsync(response, cancellationToken);
		return (await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken))!;
	}

	private async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		if (response.IsSuccessStatusCode)
			return;

		ResponseErrorJson? error = null;
		try
		{
			error = await response.Content.ReadFromJsonAsync<ResponseErrorJson>(JsonOptions, cancellationToken);
		}
		catch (JsonException) { }
		catch (NotSupportedException) { }

		if (response.StatusCode == HttpStatusCode.Unauthorized)
			_authState.SignOut();

		var errors = error?.Errors is { Count: > 0 } list ? list : [_i18n.F("error.requestFailed", (int)response.StatusCode)];
		throw new ApiException(response.StatusCode, errors);
	}
}
