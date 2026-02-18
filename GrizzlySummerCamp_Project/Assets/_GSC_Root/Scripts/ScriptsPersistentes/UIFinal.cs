using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFinal : MonoBehaviour
{
    public static UIFinal Instance;

    [Header("UI HUB")]
    [SerializeField] private string nombreUIHub = "interfazFinal";
    [SerializeField] private string nombreUITodas = "interfazTodasInsignias";

    [Header("Condiciones")]
    [SerializeField] private string[] minijuegosInsigniaOro;
    [SerializeField] private string[] minijuegosEstrella;

    private bool yaMostrado = false;
    private bool yaMostradoTodas = false;

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

        //Prioridad para que no se muestre la de todas si se ha cosneguido oro antes
        if (!yaMostrado && CumpleCondiciones())
        {
            MostrarUI(nombreUIHub);
            yaMostrado = true;

            yaMostradoTodas = true;
            return;
        }

        if (!yaMostradoTodas && TieneTodasLasInsignias())
        {
            MostrarUI(nombreUITodas);
            yaMostradoTodas = true;
        }
    }

    private bool CumpleCondiciones()
    {
        if (InsigniaManager.Instance == null)
            return false;

        // Comprobar insignias
        foreach (string minijuego in minijuegosInsigniaOro)
        {
            int insignia = InsigniaManager.Instance.GetInsignia(minijuego);

            if (insignia < 3)
                return false;
        }

        // Comprobar estrellas
        foreach (string minijuego in minijuegosEstrella)
        {
            int estrella = InsigniaManager.Instance.GetEstrella(minijuego);

            if (estrella <= 0)
                return false;
        }

        return true;
    }

    private bool TieneTodasLasInsignias()
    {
        if (InsigniaManager.Instance == null)
            return false;

        foreach(string minijuego in minijuegosInsigniaOro)
        {
            int insignia = InsigniaManager.Instance.GetInsignia(minijuego);

            if (insignia <= 0)
                return false;
        }

        return true;
    }

    private void MostrarUI(string nombreUI)
    {
        GameObject ui = BuscarUIInclusoInactiva(nombreUI);

        if (ui == null)
        {
            return;
        }

        ui.SetActive(true);
    }

    private GameObject BuscarUIInclusoInactiva(string nombre)
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            Transform[] hijos = canvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in hijos)
            {
                if (t.name == nombre)
                    return t.gameObject;
            }
        }

        Transform[] todos = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in todos)
        {
            if (t.name == nombre)
                return t.gameObject;
        }

        return null;
    }

    public void CerrarFinalUI() 
    {
    
    
    }
}