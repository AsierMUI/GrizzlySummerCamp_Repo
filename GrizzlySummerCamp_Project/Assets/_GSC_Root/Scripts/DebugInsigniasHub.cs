using UnityEngine;

public class DebugInsigniasHub : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Minijuegos que usan insignias")]
    [SerializeField] private string[] minijuegosInsignia;

    [Tooltip("Minijuegos que usan estrella (contrarreloj)")]
    [SerializeField] private string[] minijuegosEstrella;

    [Tooltip("Tecla para activar el debug")]
    [SerializeField] private KeyCode teclaDebug = KeyCode.F9;

    private void Update()
    {
        if (Input.GetKeyDown(teclaDebug))
        {
            ForzarInsignias();
        }
    }

    private void ForzarInsignias()
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogError("[DebugInsigniasHub] InsigniaManager no encontrado");
            return;
        }

        // Forzar insignias a oro
        foreach (string minigame in minijuegosInsignia)
        {
            InsigniaManager.Instance.GuardarInsignia(minigame, 3);
            Debug.Log($"[Debug] Insignia ORO forzada → {minigame}");
        }

        // Forzar estrellas
        foreach (string minigame in minijuegosEstrella)
        {
            InsigniaManager.Instance.GuardarEstrella(minigame, 1);
            Debug.Log($"[Debug] Estrella forzada → {minigame}");
        }

        Debug.Log("[DebugInsigniasHub] TODAS las insignias forzadas");
    }
}