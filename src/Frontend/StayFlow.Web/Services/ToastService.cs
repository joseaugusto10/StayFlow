using StayFlow.Web.Services.Localization;

namespace StayFlow.Web.Services;

public enum ToastKind { Success, Error, Info }

public sealed record ToastMessage(Guid Id, ToastKind Kind, string Text);

public sealed class ToastService
{
	private readonly List<ToastMessage> _toasts = [];
	private readonly I18n _i18n;

	public ToastService(I18n i18n)
	{
		_i18n = i18n;
	}

	public IReadOnlyList<ToastMessage> Toasts => _toasts;
	public event Action? Changed;

	public void Success(string text) => Show(ToastKind.Success, text);
	public void Info(string text) => Show(ToastKind.Info, text);
	public void Error(string text) => Show(ToastKind.Error, text);
	public void Error(Exception exception) => Show(ToastKind.Error, exception is ApiException api ? string.Join(" ", api.Errors) : _i18n["error.unexpected"]);

	public void Dismiss(Guid id)
	{
		_toasts.RemoveAll(toast => toast.Id == id);
		Changed?.Invoke();
	}

	private void Show(ToastKind kind, string text)
	{
		var toast = new ToastMessage(Guid.NewGuid(), kind, text);
		_toasts.Add(toast);
		Changed?.Invoke();

		_ = Task.Delay(4500).ContinueWith(_ => Dismiss(toast.Id));
	}
}
