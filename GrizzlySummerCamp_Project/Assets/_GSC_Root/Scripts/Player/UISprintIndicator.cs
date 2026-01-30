using UnityEngine;

public class UISprintIndicator : MonoBehaviour
{
    [SerializeField] private GameObject sprintIcon;

    private void OnEnable()
    {
        PlayerMovement.OnRunningChanged += OnRunningChanged;
    }

    private void OnDisable()
    {
        PlayerMovement.OnRunningChanged -= OnRunningChanged;
    }

    void OnRunningChanged(bool isRunning) 
    {
        if (sprintIcon == null) return;

        if (UIState.IsUIOpen)
        {
            sprintIcon.SetActive(false);
            return;
        }

        sprintIcon.SetActive(isRunning);
    }

}
