using UnityEngine;
using UnityEngine.UI;

public class MedallaHub : MonoBehaviour
{
    [Header("Sprites Medallas")]
    [SerializeField] private Image medallaImage;
    [SerializeField] private Sprite medallaBronce;
    [SerializeField] private Sprite medallaPlata;
    [SerializeField] private Sprite medallaOro;

    void Start() 
    {
        int mejorMedalla = PlayerPrefs.GetInt("MejorMedallaPesca", 0);
        switch (mejorMedalla)
        {
            case 3: medallaImage.sprite = medallaOro; break;
            case 2: medallaImage.sprite = medallaPlata; break;
            case 1: medallaImage.sprite = medallaBronce; break;
            default: medallaImage.sprite = null; break;
        }
    }
}
