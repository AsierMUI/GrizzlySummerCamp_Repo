using UnityEngine;

public class DebugInsigniasHub : MonoBehaviour
{
    [Header("Debug")]
    [Tooltip("Minijuegos que usan insignias")]
    [SerializeField] private string[] minijuegosInsignia;

    [Tooltip("Minijuegos que usan estrella (contrarreloj)")]
    [SerializeField] private string[] minijuegosEstrella;

    [Header("Teclas Debug")]
    [SerializeField] private KeyCode teclaOro = KeyCode.F9;
    [SerializeField] private KeyCode teclaBronce = KeyCode.F8;

    private void Update()
    {
        if (Input.GetKeyDown(teclaOro))
        {
            ForzarInsignias(3); // ORO
        }

        if (Input.GetKeyDown(teclaBronce))
        {
            ForzarInsignias(1); // BRONCE
        }
    }

    private void ForzarInsignias(int nivelInsignia)
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogError("[DebugInsigniasHub] InsigniaManager no encontrado");
            return;
        }

        // Forzar insignias (1 = bronce, 3 = oro)
        foreach (string minigame in minijuegosInsignia)
        {
            InsigniaManager.Instance.GuardarInsignia(minigame, nivelInsignia);
            Debug.Log($"[Debug] Insignia {(nivelInsignia == 3 ? "ORO" : "BRONCE")} forzada → {minigame}");
        }

        // Forzar estrellas
        foreach (string minigame in minijuegosEstrella)
        {
            InsigniaManager.Instance.GuardarEstrella(minigame, 1);
            Debug.Log($"[Debug] Estrella forzada → {minigame}");
        }

        Debug.Log($"[DebugInsigniasHub] TODAS las insignias forzadas a {(nivelInsignia == 3 ? "ORO" : "BRONCE")}");
    }
}