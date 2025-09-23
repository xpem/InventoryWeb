namespace InventoryWeb.Infra.Services;

public class NavMenuService
{
    public event Action? OnToggle;

    public void ToggleNavMenu()
    {
        OnToggle?.Invoke();
    }
}