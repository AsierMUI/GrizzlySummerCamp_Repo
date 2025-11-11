using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MostrarInsigniaHub : MonoBehaviour
{
    [Header("Insignias pesca")]
    [SerializeField] private Image insigniaImage;
    [SerializeField] private Sprite insignianull;
    [SerializeField] private Sprite insigniaBronce;
    [SerializeField] private Sprite insigniaPlata;
    [SerializeField] private Sprite insigniaOro;

    [Header("Estrella")]
    [SerializeField] private Image estrellaImage;
    [SerializeField] private Sprite estrellaNull;
    [SerializeField] private Sprite estrellaSprite;

    private void OnEnable()
    {
        BuscarReferenciasUI(); //  Se asegura de tener las imágenes correctas
        ActualizarVisual();
    }

    private void Start()
    {
        BuscarReferenciasUI();
        ActualizarVisual();
    }
    private void BuscarReferenciasUI()
    {
        // Busca los objetos por nombre si no están asignados manualmente
        if (insigniaImage == null)
        {
            GameObject obj = GameObject.Find("InsigniaImage"); //  cambia el nombre al que uses en tu Canvas
            if (obj != null)
            {
                insigniaImage = obj.GetComponent<Image>();
                Debug.Log("[MostrarInsigniaHub] Se reasignó la imagen de insignia automáticamente.");
            }
            else
            {
                Debug.LogWarning("[MostrarInsigniaHub] No se encontró el objeto 'InsigniaImage'.");
            }
        }

        if (estrellaImage == null)
        {
            GameObject obj = GameObject.Find("EstrellaImage"); //  cambia el nombre al real en tu escena
            if (obj != null)
            {
                estrellaImage = obj.GetComponent<Image>();
                Debug.Log("[MostrarInsigniaHub] Se reasignó la imagen de estrella automáticamente.");
            }
            else
            {
                Debug.LogWarning("[MostrarInsigniaHub] No se encontró el objeto 'EstrellaImage'.");
            }
        }
    }

    private void ActualizarVisual()
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogWarning("[MostrarInsigniaHub] No se encontró InsigniaManager.");
            return;
        }

        if (insigniaImage == null)
        {
            Debug.LogWarning("[MostrarInsigniaHub] No se asignó la imagen de la insignia.");
            return;
        }

        int nivel = InsigniaManager.Instance.ultimaInsignia;
        Debug.Log($"[MostrarInsigniaHub] Mostrando insignia nivel {nivel}");

        switch (nivel)
        {
            case 1:
                insigniaImage.sprite = insigniaBronce;
                break;
            case 2:
                insigniaImage.sprite = insigniaPlata;
                break;
            case 3:
                insigniaImage.sprite = insigniaOro;
                break;
            default:
                insigniaImage.sprite = insignianull;
                break;
        }

        if (estrellaImage != null)
        {
            int tieneEstrella = InsigniaManager.Instance.ultimaEstrella;
            estrellaImage.sprite = (tieneEstrella > 0) ? estrellaSprite : estrellaNull;
        }
    }
}
