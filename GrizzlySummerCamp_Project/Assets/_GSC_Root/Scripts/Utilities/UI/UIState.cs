public static class UIState
{
    public static bool IsUIOpen = false;
    public static event System.Action<bool> OnUIStateChanged;

    public static void SetUIOpen(bool value)
    {
        if (IsUIOpen == value) return;

        IsUIOpen = value;
        OnUIStateChanged?.Invoke(IsUIOpen);
    }

}
