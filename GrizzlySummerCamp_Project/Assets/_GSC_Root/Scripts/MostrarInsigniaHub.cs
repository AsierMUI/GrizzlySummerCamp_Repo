using UnityEngine;
using UnityEngine.UI;

public class MostrarInsigniaHub : MonoBehaviour
{
    [SerializeField] private Image insigniaImage;
    [SerializeField] private Sprite insignianull;
    [SerializeField] private Sprite insigniaBronce;
    [SerializeField] private Sprite insigniaPlata;
    [SerializeField] private Sprite insigniaOro;

    private void OnEnable()
    {
        ActualizarInsignia();
    }
    private void Start()
    {
        ActualizarInsignia();
    }

    void ActualizarInsignia()
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogWarning("[MostrarInsigniaHub] No se encontró GameData.");
            return;
        }

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
    }

}
