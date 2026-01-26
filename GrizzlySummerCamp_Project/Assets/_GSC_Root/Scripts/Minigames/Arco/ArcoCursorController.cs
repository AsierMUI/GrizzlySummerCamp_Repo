using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcoCursorController : MonoBehaviour
{
    private bool isArcoScene;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnlockCursor(); 
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isArcoScene = scene.name == "SCN_MArco";

        if (isArcoScene)
            LockCursor();
        else
            UnlockCursor();
    }

    public void OnPauseOpened()
    {
        if (isArcoScene)
            UnlockCursor();
    }

    public void OnPauseClosed()
    {
        if (isArcoScene)
            LockCursor();
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
