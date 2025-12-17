using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [Header("Refs UI")]
    public GameObject menuPausa;
    public GameObject loadingUI;

    [Header("Estado")]
    public bool juegoPausado = false;

    [SerializeField] int EscenaJuego = 2;

    private void Start()
    {
        Time.timeScale = 1f;
        juegoPausado = false;

        if (menuPausa != null)
            menuPausa.SetActive(false);
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != EscenaJuego) return;

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

        if (MinigameManager.Instance != null) MinigameManager.Instance.SetPlayerMovement(false);

        Debug.Log("Juego pausado");

    }

    public void Reanudar()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;

        if (MinigameManager.Instance != null) MinigameManager.Instance.SetPlayerMovement(true);

        Debug.Log("Juego reanudado");

    }
}
