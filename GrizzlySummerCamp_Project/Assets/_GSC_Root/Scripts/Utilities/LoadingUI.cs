using System.Collections;
using TMPro;
using UnityEngine;
public class LoadingUI : MonoBehaviour
{
    [SerializeField] float duracion = 5f;
    [SerializeField] private TextMeshProUGUI loadingText;

    private string baseText = "Generating Maze";
    private int dotCount = 0;
    private LTDescr loopTween;

    void Start()
    {
        AnimateDots();
        StartCoroutine(DesactivarTrasTiempo());
    }

    void AnimateDots()
    {
        //animacion de puntos usando leantween como temporizador
        loopTween = LeanTween.value(gameObject, 0, 1, 0.5f)
            .setOnComplete(() =>
            {
                dotCount = (dotCount + 1) % 4; //esto hace quesu orden sea -> 0 -> 1 -> 2 -> 3 -> 0
                loadingText.text = baseText + new string('.', dotCount);
                AnimateDots();
            });
    }
    IEnumerator DesactivarTrasTiempo()
    {
        yield return new WaitForSeconds(duracion);

        if (loopTween != null)
            LeanTween.cancel(gameObject);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (loopTween != null)
            LeanTween.cancel(gameObject);
    }
}
