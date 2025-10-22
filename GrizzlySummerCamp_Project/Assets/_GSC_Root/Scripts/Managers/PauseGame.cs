using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject menuPausa;
    public GameObject canvasInstrucciones;
    public bool juegoPausado = false;


    private void Start()
    {
        Time.timeScale = 1;
        juegoPausado = false;
        menuPausa.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !canvasInstrucciones.activeSelf)
        {
            TogglePausa();
        }
    }

    void TogglePausa()
    {
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        menuPausa.SetActive(false);
    }

    public void Pausar()
    {
        Time.timeScale = 0f;
        juegoPausado = true;
        menuPausa.SetActive(true);
    }

    private void OnDisable()
    {
        if (menuPausa != null && !menuPausa.activeSelf && juegoPausado)
        {
            Time.timeScale = 1f;
            juegoPausado = false;
        }
    }
}
