namespace InventoryWeb.Infra.Services
{

    public record ToastComponent(string Message,string Style);

    public class ToastService
    {
        public event Action OnToastsUpdated;
        private readonly List<ToastComponent> _toasts = new();

        public IReadOnlyList<ToastComponent> Toasts => _toasts;

        public void ShowToast(ToastComponent toastComponent)
        {
            _toasts.Add(toastComponent);
            OnToastsUpdated?.Invoke();
        }

        public void RemoveToast(ToastComponent toastComponent)
        {
            _toasts.Remove(toastComponent);
            OnToastsUpdated?.Invoke();
        }
    }
}
