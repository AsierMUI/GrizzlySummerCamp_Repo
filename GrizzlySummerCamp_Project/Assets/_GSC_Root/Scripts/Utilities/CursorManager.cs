using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Interfaces que muestran cursor")]
    public GameObject loadingUI;
    public GameObject pauseUI;
    public GameObject endMinigameUI;

    [Header("Cursores")]
    public Texture2D normalCursor;
    public Texture2D invisibleCursor;

    bool cursorVisible;

    private void Start()
    {
        SetCursor(false); 
    }

    private void Update()
    {
        bool mostrarCursor = loadingUI.activeInHierarchy || endMinigameUI.activeInHierarchy || pauseUI.activeInHierarchy;

        if (mostrarCursor != cursorVisible)
        {
            SetCursor(mostrarCursor);
        }
    }

    void SetCursor(bool visible)
    {
        cursorVisible = visible;

        Cursor.SetCursor(visible ? normalCursor : invisibleCursor, Vector2.zero, CursorMode.Auto);
    }
}