using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Globalization;

namespace StayFlow.Web.Auth;

public sealed class CookieHandler : DelegatingHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
		request.Headers.AcceptLanguage.ParseAdd(CultureInfo.CurrentUICulture.Name is { Length: > 0 } culture ? culture : "en");
		return base.SendAsync(request, cancellationToken);
	}
}
