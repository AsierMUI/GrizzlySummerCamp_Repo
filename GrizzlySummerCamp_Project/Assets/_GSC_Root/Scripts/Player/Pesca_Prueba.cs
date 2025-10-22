using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Pesca_Prueba : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [Header("Fish Stats")]
    [SerializeField] Transform fish;
    float fishPosition;
    float fishDestination;
    [SerializeField] float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;
    [SerializeField] float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [Header("Hook Stats")]
    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 0.3f;
    [SerializeField] float hookProgressLossSpeed = 0.1f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float hookProgressDegradationPower = 0.1f;

    [SerializeField] Image hookImage;

    [SerializeField] Transform progressBarContainer;


    [SerializeField] float failTimer = 40f;

    [Header("UI References")]
    [SerializeField] GameObject fishingUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] TMPro.TextMeshProUGUI resultText;


    [SerializeField] float timeBetweenBites = 5f;
    float biteTimer;
    bool isFishing = false;

    [SerializeField] float resultDisplayTime = 3f;

    [Header("Instructions UI")]
    [SerializeField] GameObject instruccionesUI;

    [Header("Fishing Time Limit")]
    [SerializeField] float totalFishingTime = 120f;
    float fishingTimer;

    [Header("Score System")]
    [SerializeField] TextMeshProUGUI puntosTexto;
    [SerializeField] Image insigniaImage;

    [SerializeField] Sprite insigniaBronce;
    [SerializeField] Sprite insigniaPlata;
    [SerializeField] Sprite insigniaOro;
    int puntos = 0;

    bool minijuegoTerminado = false;

    [SerializeField] TMPro.TextMeshProUGUI mensajeFinalText;

    void Start()
    {
        fishPosition = UnityEngine.Random.Range(0f, 1f);
        fishDestination = fishPosition;

        biteTimer = timeBetweenBites;

        fishingTimer = totalFishingTime;

        if(instruccionesUI != null)
        instruccionesUI.SetActive(true);
    }
    void Update()
    {
        if (instruccionesUI != null && instruccionesUI.activeSelf)
            return;

        if(!minijuegoTerminado)
        {
            if (fishingTimer > 0f)
            {
                fishingTimer -= Time.deltaTime;
            }

            if (fishingTimer <= 0f)
            {
                minijuegoTerminado = true;
                MostrarResultadoFinal();
                return;
            }
        }
        else
        {
            return;
        }

        if (!isFishing)
        {
            biteTimer -= Time.deltaTime;
            if (biteTimer <= 0f)
            {
                StartFishing();
            }
            return;
        }

        Fish();
        Hook();
        ProgressCheck();
    }

    private void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 1;
        float max = hookPosition + hookSize / 1;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress = Mathf.MoveTowards(hookProgress, 1f,hookPower * Time.deltaTime);
        }
        else
        {
            hookProgress = Mathf.Lerp(hookProgress, 0f, hookProgressLossSpeed * Time.deltaTime);

            failTimer -= Time.deltaTime;
            if (failTimer < 0)
            {
                Lose();
            }
        }

        if(hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    private void Win()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "Got it!";

        puntos += 100;

        if (puntosTexto != null)
            puntosTexto.text = "Points:" + puntos.ToString();

        ActualizarInsignia();

        StartCoroutine(HideResultUIAfterDelay());
    }

    private void Lose()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "It escaped :(";
        StartCoroutine(HideResultUIAfterDelay());
    }

    IEnumerator HideResultUIAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        resultUI.SetActive(false);
    }
    void Hook() 
    {
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        hookPullVelocity -= hookGravityPower * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if(hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }

    if(hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }
    void Fish() 
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
    void StartFishing()
    {
        isFishing = true;
        biteTimer = timeBetweenBites;

        fishingUI.SetActive(true);
        resultUI.SetActive(false);

        hookProgress = 0f;
        failTimer = 10f;

        fishPosition = UnityEngine.Random.Range(0f, 1f);
        fishDestination = fishPosition;
    }

    void ActualizarInsignia()
    {
        if (insigniaImage == null) return;

        if (puntos >= 300)
        {
            insigniaImage.sprite = insigniaOro;
        }
        else if (puntos == 200)
        {
            insigniaImage.sprite = insigniaPlata;
        }
        else if (puntos == 100)
        {
            insigniaImage.sprite = insigniaBronce;
        }
        else
        {
            insigniaImage.sprite = null;
        }
    }

    void MostrarResultadoFinal()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);

        string mensaje = "";

        if (puntos >= 300)
            mensaje = "¡Wow!";
        else if (puntos == 200)
            mensaje = "Incredible!";
        else if (puntos == 100)
            mensaje = "Well done!";
        else
            mensaje = "Oops :(";
        if(mensajeFinalText != null)
            mensajeFinalText.text = mensaje;

    }

}
