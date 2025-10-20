using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject menuPausa;
    public bool juegoPausado = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false); //quita el menu de pausa
        Time.timeScale = 1; //el juego avanza a velocidad normal
        juegoPausado = false; //indica que el juego no esta pausado
    }

    public void Pausar()
    {
        menuPausa.SetActive(true); //activa el menu de pausa
        Time.timeScale = 0; //el juego no avanza
        juegoPausado = true; //indica que el juego esta pausado
    }
}
