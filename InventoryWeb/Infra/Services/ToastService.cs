namespace InventoryWeb.Infra.Services
{
    public class ToastService
    {
        public event Action OnToastsUpdated;
        private readonly List<string> _toasts = new();

        public IReadOnlyList<string> Toasts => _toasts;

        public void ShowToast(string message)
        {
            _toasts.Add(message);
            OnToastsUpdated?.Invoke();
        }

        public void RemoveToast(string message)
        {
            _toasts.Remove(message);
            OnToastsUpdated?.Invoke();
        }
    }
}
