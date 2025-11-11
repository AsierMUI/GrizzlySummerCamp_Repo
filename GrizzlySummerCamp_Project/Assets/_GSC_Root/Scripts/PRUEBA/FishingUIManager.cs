using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FishingUIManager : MonoBehaviour
{
    //MANEJA LA INTERFAZ(RESULTADO,PUNTOS,INSIGNIAS)

    public GameObject miniResultUI;
    public TextMeshProUGUI resultText;
    public float resultDisplayTime = 1f;

    public TextMeshProUGUI puntosTexto;
    public Image insigniaImage;
    public Sprite insigniaBronce;
    public Sprite insigniaPlata;
    public Sprite insigniaOro;

    private int puntos = 0;

    private void OnEnable()
    {
        EventManager.OnFishCaught += GanarPuntos;
        EventManager.OnFishEscaped += MostrarEscape;
    }

    private void OnDisable()
    {
        EventManager.OnFishCaught -= GanarPuntos;
        EventManager.OnFishEscaped -= MostrarEscape;
    }

    void GanarPuntos(Dificultad dificultad)
    {
        miniResultUI.SetActive(true);
        resultText.text = "Got it!";

        int puntosGanados = dificultad switch
        {
            Dificultad.Facil => 50,
            Dificultad.Normal => 100,
            Dificultad.Dificil => 200,
            _ => 100
        };

        puntos += puntosGanados;
        puntosTexto.text = "Points: " + puntos;
        ActualizarInsignia();

        StartCoroutine(HideResultUIAfterDelay());
    }

    void MostrarEscape()
    {
        miniResultUI.SetActive(true);
        resultText.text = "It escaped :(";
        StartCoroutine(HideResultUIAfterDelay());
    }

    IEnumerator HideResultUIAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        miniResultUI.SetActive(false);
    }

    void ActualizarInsignia()
    {
        if (insigniaImage == null) return;

        if (puntos >= 300) insigniaImage.sprite = insigniaOro;
        else if (puntos >= 200) insigniaImage.sprite = insigniaPlata;
        else if (puntos >= 100) insigniaImage.sprite = insigniaBronce;
        else insigniaImage.sprite = null;
    }
}
