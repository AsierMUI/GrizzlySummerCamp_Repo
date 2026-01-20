using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFinal : MonoBehaviour
{
    public static UIFinal Instance;
    [Header("Conf")]
    [SerializeField] private string nommbreUIHub = "UIFinal";
    [SerializeField] private string[] minigamesRequeridos;

    private bool yaMostrado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "SCN_HUB") return;

        if (yaMostrado) return;

        if (CumpleCondiciones())
        {
            MostrarUIEnHub();
            yaMostrado = true;
        }
    }

    private bool CumpleCondiciones()
    {
        if (InsigniaManager.Instance == null)
            return false;

        foreach(string minigame in minigamesRequeridos)
        {
            int insignia = InsigniaManager.Instance.GetInsignia(minigame);
            int estrella = InsigniaManager.Instance.GetEstrella(minigame);

            if (insignia < 3 || estrella <= 0)
            {
                return false;
            }
        }

        return true;
    }

    private void MostrarUIEnHub()
    {
        GameObject ui = GameObject.Find(nommbreUIHub);

        if (ui == null)
        {
            Debug.LogWarning($"no se ha encontrado la interfaz {nommbreUIHub}");
            return;
        }

        ui.SetActive(true);
    }
}