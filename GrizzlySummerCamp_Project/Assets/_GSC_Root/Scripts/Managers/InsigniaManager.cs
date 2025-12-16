using System.Collections.Generic;
using UnityEngine;

public class InsigniaManager : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS

    public static InsigniaManager Instance;

    //Diccionario: clave = nombre del minijuego, valor = insignia maxima(0 nada 1 bronze, etc)
    private Dictionary<string, int> minigameInsignias = new Dictionary<string, int>();

    //Diccionario: clave = nombre del minijuego, valor = estrella obtenida(0 no 1 si)
    private Dictionary<string, int> minigameEstrellas = new Dictionary<string, int>();

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

    //INSIGNIAS
    public void GuardarInsignia(string minigameName, int nuevaInsignia)
    {
        if (!minigameInsignias.ContainsKey(minigameName))
            minigameInsignias[minigameName] = 0;

        if (nuevaInsignia > minigameInsignias[minigameName])
        {
            minigameInsignias[minigameName] = nuevaInsignia;
            Debug.Log($"[InsigniaManager] Nueva insignia para {minigameName}: {nuevaInsignia}");
        }
    }

    public int GetInsignia(string minigameName)
    {
        if (minigameInsignias.ContainsKey(minigameName))
            return minigameInsignias[minigameName];
        return 0;
    }

    //ESTRELLA
    public void GuardarEstrella(string minigameName, int nuevaEstrella)
    {
        if (!minigameEstrellas.ContainsKey(minigameName))
            minigameEstrellas[minigameName] = 0;

        if (nuevaEstrella > minigameEstrellas[minigameName])
        {
            minigameEstrellas[minigameName] = nuevaEstrella;
            Debug.Log($"[InsigniaManager] Nueva estrella para {minigameName}:{nuevaEstrella}");
        }
    }

    public int GetEstrella(string minigameName)
    {
        if (minigameEstrellas.ContainsKey(minigameName))
            return minigameEstrellas[minigameName];
        return 0;
    }

    public Dictionary<string, int> GetAllInsignias() => new Dictionary<string, int>(minigameInsignias);
    public Dictionary<string, int> GetAllEstrellas() => new Dictionary<string, int>(minigameEstrellas);

}
