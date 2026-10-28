using Microsoft.JSInterop;
using System.Globalization;

namespace StayFlow.Web.Services.Localization;

public sealed record LanguageOption(string Culture, string Flag, string Name);

public sealed class I18n
{
	private const string STORAGE_KEY = "stayflow.lang";
	public const string DEFAULT_CULTURE = "pt-BR";

	public static readonly IReadOnlyList<LanguageOption> Languages =
	[
		new("pt-BR", "🇧🇷", "Português"),
		new("en", "🇺🇸", "English"),
		new("es", "🇪🇸", "Español")
	];

	private readonly IJSRuntime _js;

	public I18n(IJSRuntime js)
	{
		_js = js;
	}

	public string Culture { get; private set; } = DEFAULT_CULTURE;
	public event Action? Changed;

	public string this[string key] => Translations.Get(Culture, key);

	public string Enum<TEnum>(TEnum value) where TEnum : struct, System.Enum =>
		this[$"{typeof(TEnum).Name}.{value}"];

	public string F(string key, params object?[] args) =>
		string.Format(CultureInfo.CurrentCulture, this[key], args);

	public async Task InitializeAsync()
	{
		string? stored = null;
		try
		{
			stored = await _js.InvokeAsync<string?>("localStorage.getItem", STORAGE_KEY);
			stored ??= await _js.InvokeAsync<string?>("eval", "navigator.language");
		}
		catch (JSException) { }

		Apply(Normalize(stored));
	}

	public async Task SetCultureAsync(string culture)
	{
		culture = Normalize(culture);
		if (culture == Culture)
			return;

		Apply(culture);

		try { await _js.InvokeVoidAsync("localStorage.setItem", STORAGE_KEY, culture); }
		catch (JSException) { }

		Changed?.Invoke();
	}

	private void Apply(string culture)
	{
		Culture = culture;
		var info = new CultureInfo(culture);
		CultureInfo.DefaultThreadCurrentCulture = info;
		CultureInfo.DefaultThreadCurrentUICulture = info;
		CultureInfo.CurrentCulture = info;
		CultureInfo.CurrentUICulture = info;
	}

	private static string Normalize(string? culture) => culture?.ToLowerInvariant() switch
	{
		null or "" => DEFAULT_CULTURE,
		var c when c.StartsWith("pt") => "pt-BR",
		var c when c.StartsWith("es") => "es",
		var c when c.StartsWith("en") => "en",
		_ => DEFAULT_CULTURE
	};
}
