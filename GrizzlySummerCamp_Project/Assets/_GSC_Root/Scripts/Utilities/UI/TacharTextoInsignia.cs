using UnityEngine;
using TMPro;

public class TacharTextoInsignia : MonoBehaviour
{
    public enum TipoDesbloqueo
    {
        Insignia,
        Estrella
    }

    [System.Serializable]
    public class TextoMinijuego
    {
        public string minigameName;
        public TipoDesbloqueo tipoDesbloqueo;
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
            if (t.texto == null) continue;

            bool desbloqueado = false;

            switch (t.tipoDesbloqueo)
            {
                case TipoDesbloqueo.Insignia:
                    desbloqueado = InsigniaManager.Instance.GetInsignia(t.minigameName) > 0;
                    break;

                case TipoDesbloqueo.Estrella:
                    desbloqueado = InsigniaManager.Instance.GetEstrella(t.minigameName) > 0;
                    break;
            }

            t.texto.fontStyle = desbloqueado
                ? FontStyles.Strikethrough : FontStyles.Normal;

            Debug.Log($"{t.minigameName} [{t.tipoDesbloqueo}] -> {desbloqueado}");
        }
    }
}