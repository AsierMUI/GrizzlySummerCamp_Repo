using UnityEngine;

public class PersistentRoot : MonoBehaviour
{
    // Unico script que debe contener  DontDestroyOnLoad, solo en empty llamado RootPersistente
    // en el cual se deben añadir otros emptys de scripts que necesiten permanecer entre escenas

    private static PersistentRoot instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}