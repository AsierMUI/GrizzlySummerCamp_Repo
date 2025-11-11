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
        ActualizarVisual();
    }
    private void Start()
    {
        ActualizarVisual();
    }

    void ActualizarVisual()
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogWarning("[MostrarInsigniaHub] No se encontró GameData.");
            return;
        }

        //Insignia
        if (insigniaImage == null)
        {
            Debug.LogWarning("[MostrarInsigniaHub] No se asignó la imagen dela insignia.");
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

        //Estrella
        if (estrellaImage != null)
        {
            int tieneEstrella = InsigniaManager.Instance.ultimaEstrella;

            if (tieneEstrella > 0)
                estrellaImage.sprite = estrellaSprite;
            else
                estrellaImage.sprite = estrellaNull;
        }
    }
}
