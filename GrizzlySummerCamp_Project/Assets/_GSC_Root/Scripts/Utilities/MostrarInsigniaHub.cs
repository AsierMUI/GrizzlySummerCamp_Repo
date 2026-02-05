using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MostrarInsigniaHub : MonoBehaviour
{
    #region Clases y variables
    [System.Serializable]
    public class MinigameUI
    {
        public string minigameName;
        public Image insigniaImage;
        public Sprite insigniaNull;
        public Sprite insigniaBronce;
        public Sprite insigniaPlata;
        public Sprite insigniaOro;

        public Image estrellaImage;
        public Sprite estrellaNull;
        public Sprite estrellaSprite;
    }

    [Header("Minijuegos a mostrar")]
    public MinigameUI[] minijuegos;
    #endregion

    #region Unity Callbacks
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
        if (scene.name == "SCN_HUB")
        {
            ActualizarVisuales();
        }
    }
    #endregion

    #region Funciones
    private void ActualizarVisuales()
    {
        if (InsigniaManager.Instance == null)
        {
            return;
        }

        foreach (var mg in minijuegos)
        {
            if (mg.insigniaImage != null)
            {
                int nivel = InsigniaManager.Instance.GetInsignia(mg.minigameName);
                switch (nivel)
                {
                    case 1: mg.insigniaImage.sprite = mg.insigniaBronce; break;
                    case 2: mg.insigniaImage.sprite = mg.insigniaPlata; break;
                    case 3: mg.insigniaImage.sprite = mg.insigniaOro; break;
                    default: mg.insigniaImage.sprite = mg.insigniaNull; break;
                }
            }

            if (mg.estrellaImage != null)
            {
                int estrella = InsigniaManager.Instance.GetEstrella(mg.minigameName);
                mg.estrellaImage.sprite = (estrella > 0) ? mg.estrellaSprite : mg.estrellaNull;
            }
        }
    }
    #endregion
}