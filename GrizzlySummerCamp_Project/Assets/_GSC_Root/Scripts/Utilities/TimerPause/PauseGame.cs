using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [Header("Refs UI")]
    public GameObject menuPausa;
    public GameObject loadingUI;

    [Header("Estado")]
    public bool juegoPausado = false;

    private PlayerMovement playerMovement;

    private void Start()
    {
        Time.timeScale = 1f;
        juegoPausado = false;

        if (menuPausa != null)
            menuPausa.SetActive(false);

        playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerMovement == null)
            Debug.LogWarning("PauseGame no se ha encontrado playermovement en escena");
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && (loadingUI == null || !loadingUI.activeSelf))
        {
            TogglePausa();
        }
    }

    void TogglePausa()
    {
        if (juegoPausado)
            Reanudar();
        else
            Pausar();
    }
    public void Pausar()
    {
        if (menuPausa !=null)
            menuPausa.SetActive(true);

        Time.timeScale = 0f;
        juegoPausado = true;

        if (playerMovement != null) playerMovement.SetCanMove(false);
    }

    public void Reanudar()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;

        if (playerMovement != null) playerMovement.SetCanMove(true);
    }
}
