using UnityEngine;
using TMPro;

public class TacharTextoInsignia : MonoBehaviour
{
    [System.Serializable]
    public class TextoMinijuego
    {
        public string minigameName;
        public TextMeshProUGUI texto;
    }

    [Header("Textos a tachar")]
    public TextoMinijuego[] textos;

    void OnEnable()
    {
        ActualizarTextos();
    }

    public void ActualizarTextos()
    {
        if (InsigniaManager.Instance == null)
        {
            Debug.LogWarning("[TacharTextoInsignia] InsigniaManager no encontrao");
            return;
        }

        foreach (var t in textos)
        {
            int insignia = InsigniaManager.Instance.GetInsignia(t.minigameName);
            Debug.Log($"{t.minigameName} -> Insignia: {insignia}");

            if (t.texto == null) continue;

            if (insignia > 0)
            {
                t.texto.fontStyle |= FontStyles.Strikethrough;
            }
            else
            {
                t.texto.fontStyle &= ~FontStyles.Strikethrough;
            }
        }
    }
}