using Microsoft.AspNetCore.Components;
using StayFlow.Web.Services.Localization;

namespace StayFlow.Web.Components;

public abstract class LocalizedLayout : LayoutComponentBase, IDisposable
{
	[Inject] protected I18n L { get; set; } = default!;

	protected override void OnInitialized()
	{
		L.Changed += OnLanguageChanged;
		base.OnInitialized();
	}

	private void OnLanguageChanged() => InvokeAsync(StateHasChanged);

	public virtual void Dispose() => L.Changed -= OnLanguageChanged;
}
