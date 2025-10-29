using UnityEngine;

public class PauseGame : MonoBehaviour
{

    [Header("Refs UI")]
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
        if (Input.GetKeyDown(KeyCode.Escape) &&
            (canvasInstrucciones == null || !canvasInstrucciones.activeSelf))
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

        var boat = FindFirstObjectByType<BoatMovement>();
        if (boat != null)
        {
            boat.canMove = false;
            boat.ResetVelocity();
        }

        Debug.Log("Juego pausado. Time.timeScale = " + Time.timeScale);

    }

    public void Reanudar()
    {
        if (menuPausa != null)
            menuPausa.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;

        var boat = FindFirstObjectByType<BoatMovement>();
        boat.canMove = true;

        Debug.Log("Juego reanudado. Time.timeScale = " + Time.timeScale);

    }
}
