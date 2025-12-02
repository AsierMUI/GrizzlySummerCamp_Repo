using UnityEngine;

public class OcultaInstrucciones : MonoBehaviour
{
    public static OcultaInstrucciones Instance;

    [SerializeField] private GameObject instrucciones;

    private static bool instruccionesOcultasEnSesion = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
                return;
    }
    private void Start()
    {
        if (instrucciones != null)
            instrucciones.SetActive(!instruccionesOcultasEnSesion);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OcultarInstrucciones();
        }
    }

    public void OcultarInstrucciones()
    {
        if (instrucciones == null) return;

        RectTransform rt = instrucciones.GetComponent<RectTransform>();

        rt.localScale = Vector3.one;
        LeanTween.scale(rt, Vector3.one * 0.1f, 0.2f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() =>
                {
                    instrucciones.SetActive(false);
                    instruccionesOcultasEnSesion = true;

                });
    }
}
