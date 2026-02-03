using UnityEngine;

public class UISprintIndicator : MonoBehaviour
{
    [SerializeField] private GameObject sprintIcon;

    private bool isRunning;

    private void OnEnable()
    {
        PlayerMovement.OnRunningChanged += OnRunningChanged;
        UIState.OnUIStateChanged += OnUIStateChanged;
    }

    private void OnDisable()
    {
        PlayerMovement.OnRunningChanged -= OnRunningChanged;
        UIState.OnUIStateChanged -= OnUIStateChanged;
    }

    void OnRunningChanged(bool running) 
    {
        isRunning = running;
        Refresh();
    }

    void OnUIStateChanged(bool uiOpen)
    {
        Refresh();
    }

    void Refresh()
    {
        if (sprintIcon == null) return;

        sprintIcon.SetActive(isRunning && !UIState.IsUIOpen);
    }
}
