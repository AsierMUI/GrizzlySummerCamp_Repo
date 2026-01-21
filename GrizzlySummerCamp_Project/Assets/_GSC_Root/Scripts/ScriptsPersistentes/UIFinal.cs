using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFinal : MonoBehaviour
{
    public static UIFinal Instance;

    [Header("UI HUB")]
    [SerializeField] private string nombreUIHub = "interfazFinal";

    [Header("Miinjuegos obligatorios")]
    [SerializeField] private string[] minijuegosObligatorios;

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
        {
            Debug.Log("[UIFinal] insigniamanager no encontrao");
            return false;
        }

        foreach(string minigame in minijuegosObligatorios)
        {
            int insignia = InsigniaManager.Instance.GetInsignia(minigame);
            int estrella = InsigniaManager.Instance.GetEstrella(minigame);

            Debug.Log($"[UIFinal] {minigame} => Insignia:{insignia} Estrella:{estrella}");

            if (insignia < 3 || estrella <= 0)
            {
                return false;
            }
        }

        return true;
    }

    private void MostrarUIEnHub()
    {
        GameObject ui = GameObject.Find(nombreUIHub);

        if (ui == null)
        {
            Debug.LogWarning($"[UIFinal] no se ha encontrado la interfaz {nombreUIHub} en el hub");
            return;
        }

        ui.SetActive(true);
        Debug.Log("[UIFinal] interfaz final mostrada");
    }
}