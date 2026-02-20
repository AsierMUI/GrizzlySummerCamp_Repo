using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EscapeManager : MonoBehaviour
{
    private void Update()
    {
        if (!Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            return;
        }

        HandleEscape();
    }

    void HandleEscape()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex)
        {
            case 0:
                HandleMenu();
                break;
            case 6:
                HandleIntro();
                break;

            case 1:
                HandleHub();
                break;

            default:
                HandleMinigame();
                break;
        }
    }

    #region Menu
    void HandleMenu() 
    {
        GameObject options = GameObject.Find("Pop-up_Settings");
        GameObject audiop = GameObject.Find("MusicSettings");
        GameObject videop = GameObject.Find("VideoSettings");
        GameObject exit = GameObject.Find("Pop-up_Exit");

        if (audiop != null && audiop.activeSelf)
        {
            audiop.SetActive(false);
            return;
        }

        if (videop != null && videop.activeSelf)
        {
            videop.SetActive(false);
            return;
        } 

        if (options != null && options.activeSelf)
        {
            options.SetActive(false);
            return;
        }
        

        if (exit != null && exit.activeSelf)
        {
            exit.SetActive(false);
            return;
        }
    }
    #endregion

    #region Intro
    void HandleIntro() 
    {
        IntroVideoController intro = FindFirstObjectByType<IntroVideoController>();
        if (intro != null)
        {
            intro.SkipIntro();
        }
    
    }
    #endregion


    #region Hub
    void HandleHub() 
    {
        DialogueSystem dialogue = FindFirstObjectByType<DialogueSystem>();
        if (dialogue != null && dialogue.IsTalking) 
        {
            return;
        }

        InteractableObject[] interactables =
            FindObjectsByType<InteractableObject>(FindObjectsSortMode.None);

        foreach (var interactable in interactables)
        {
            var instructionsField = typeof(InteractableObject)
                .GetField("InstructionsUI",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);

            if (instructionsField != null)
            {
                GameObject instructions =
                    (GameObject)instructionsField.GetValue(interactable);

                if (instructions != null && instructions.activeSelf)
                {
                    interactable.CloseUIFromButton();
                    return;
                }
            }
        }

        UIAnimations libreta = FindFirstObjectByType<UIAnimations>();
        if (libreta != null)
        {
            libreta.ToggleLibreta();
        }
    }
    #endregion

    #region Minigame(s)
    void HandleMinigame() 
    {
        PauseGame pause = FindFirstObjectByType<PauseGame>();
        if (pause != null)
        {
            pause.TogglePausa();
        }
        
    }
    #endregion

}
