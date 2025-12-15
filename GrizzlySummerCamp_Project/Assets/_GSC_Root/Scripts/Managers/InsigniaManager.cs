using Unity.VisualScripting;
using UnityEngine;

public class InsigniaManager : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS

    public static InsigniaManager Instance;
    
    public int ultimaInsignia = 0;// 0 = sin insignia, 1 = bronce, 2 = plata, 3 = oro
    public int ultimaEstrella = 0;// 0 = sin estrella, 1 = conseguida

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //hace que se mantenga entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GuardarInsignia(int nuevaInsignia)
    {
        //solo guarda la mayor rareza
        if (nuevaInsignia > ultimaInsignia)
        {
            ultimaInsignia = nuevaInsignia;
            Debug.Log($"[InsigniaManager] Nueva insignia guardada: {ultimaInsignia}");
        }
    }

    public void GuardarEstrella(int nuevaEstrella)
    {
        if (nuevaEstrella > ultimaEstrella)
        {
            ultimaEstrella = nuevaEstrella;
            Debug.Log("nuevaEstrella estrella guardada");
        }
    }

    //Funciones en minijuegos

    public void ShowInsignia(int score)
    {
        // 0-3 bronce, 4-6 plata, 7+ oro

        int insignia = 0;

        if (score >= 0 && score <= 3) insignia = 1; // bronce
        else if (score >= 4 && score <= 6) insignia = 2; // plata
        else if (score >= 7) insignia = 3; // oro

        GuardarInsignia(insignia);

        Debug.Log($"[InsigniaManager] Insignia obtenida por scroe {score}: {insignia}");
    }

}
