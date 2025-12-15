using System.Collections;
using TMPro;
using UnityEngine;
public class LoadingUI : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS
    [Header("Loading Settings")]
    [SerializeField] float duracion = 4f;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject startButton;

    [Header("Loading Texts")]
    [SerializeField] private string[] loadingTLines; //Asignar el texto en cada escena

    private string baseText;
    private int dotCount = 0;
    private LTDescr loopTween;

    void Start()
    {
        //Texto aleatorio
        baseText = loadingTLines[Random.Range(0, loadingTLines.Length)];

        if (startButton != null )
            startButton.SetActive(false);

        AnimateDots();
        StartCoroutine(ShowStartButtonAfterTime());
    }

    void AnimateDots()
    {
        //animacion de puntos usando leantween como temporizador
        loopTween = LeanTween.value(gameObject, 0, 1, 0.5f)
            .setOnComplete(() =>
            {
                dotCount = (dotCount + 1) % 4; //esto hace que su orden sea -> 0 -> 1 -> 2 -> 3 -> 0
                loadingText.text = baseText + new string('.', dotCount);
                AnimateDots();
            });
    }
    IEnumerator ShowStartButtonAfterTime()
    {
        yield return new WaitForSeconds(duracion);

        if (loopTween != null)
            LeanTween.cancel(gameObject);

        loadingText.text = "Game Ready!";

        if (startButton != null)
           startButton.SetActive(true);
    }
    //Metodo que llama el boton
    public void OnStartButtonPressed()
    {
        //Oculta loading
        gameObject.SetActive(false);

        //Arranca el timer
        MinigameTimer.instance.StartTimer();

        //Avisa al manager
        if (MinigameManager.Instance != null)
            MinigameManager.Instance.OnMinigameStarted();
    }
    private void OnDisable()
    {
        if (loopTween != null)
            LeanTween.cancel(gameObject);
    }
}
