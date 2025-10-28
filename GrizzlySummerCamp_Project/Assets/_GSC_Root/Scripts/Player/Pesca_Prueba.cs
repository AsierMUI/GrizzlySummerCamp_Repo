using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Pesca_Prueba : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] Transform topPivot; //Punto/Limite superior pez y gancho
    [SerializeField] Transform bottomPivot; //Punto/Limite inferior pez y gancho

    [Header("Fish Stats")]
    [SerializeField] Transform fish; //Posición Pescado
    float fishPosition;
    float fishDestination; //Posición a la que el pescado se quiere mover, alterna entre top y botton pivot
    [SerializeField] float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;
    [SerializeField] float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [Header("Hook Stats")]
    [SerializeField] Transform hook;
    float hookPosition; //Posición inicial del gancho
    [SerializeField] float hookSize = 0.1f; //tamaño del gancho
    [SerializeField] float hookPower = 0.3f; 
    [SerializeField] float hookProgressLossSpeed = 0.1f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f; //Simula la gravedad disminuyendo la fuerza del gancho
    [SerializeField] float hookProgressDegradationPower = 0.1f;
    [SerializeField] Image hookImage;
    [SerializeField] Transform progressBarContainer;
    [SerializeField] float failTimer;

    [Header("UI References")]
    [SerializeField] GameObject fishingUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] TMPro.TextMeshProUGUI resultText;
    [SerializeField] float resultDisplayTime = 3f;

    [Header("Instructions UI")]
    [SerializeField] GameObject instruccionesUI;

    [Header("Fishing Time Limit")]
    [SerializeField] float fishingTimer;

    [Header("Score System")]
    [SerializeField] TextMeshProUGUI puntosTexto;
    [SerializeField] Image insigniaImage; //Imagen de la insignia que se obtenga
    [SerializeField] Sprite insigniaBronce; //Referencia a bronce 3º Posición
    [SerializeField] Sprite insigniaPlata; //Referencia a plata, 2º Posición
    [SerializeField] Sprite insigniaOro; //Referencia a oro, 1º Posición
    int puntos = 0; //Contador de puntos (enteros, ie 1 != 1,00)

    bool minijuegoTerminado = false; //Booleano para controlar la finalización del juego.
    bool isFishing = false;

    [Header("UI Final")]
    [SerializeField] TMPro.TextMeshProUGUI mensajeFinalText;

    [Header("Pausa del player")]
    [SerializeField] BoatMovement playerMovementScript;
    [SerializeField] PlayerInput playerInput;

    void Start()
    {
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        if(instruccionesUI != null)
            instruccionesUI.SetActive(true);

        if (fishingUI != null)
            fishingUI.SetActive(false);

        if (resultUI !=null)
            resultUI.SetActive(false);
    }

    void Update()
    {
        if ((instruccionesUI != null && instruccionesUI.activeSelf) || minijuegoTerminado)
            return;

        if (fishingTimer > 0f)
            fishingTimer -= Time.deltaTime;
        else
        {
            minijuegoTerminado = true;
            MostrarResultadoFinal();
            return;
        }

        if (!isFishing) return;

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
            hookProgress = Mathf.MoveTowards(hookProgress, 1f, hookPower * Time.deltaTime);
        }
        else
        {
            hookProgress = Mathf.Lerp(hookProgress, 0f, hookProgressLossSpeed * Time.deltaTime);
            failTimer -= Time.deltaTime;
            if (failTimer < 0) Lose();
        }

        if (hookProgress >= 1f) Win();

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    private void Win()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "Got it!";

        puntos += 100;
        if (puntosTexto != null) puntosTexto.text = $"Points: {puntos}";
        ActualizarInsignia();

        if (playerMovementScript != null) playerMovementScript.enabled = true;

        if (playerInput != null)
        {
            var playerMap = playerInput.actions.FindActionMap("Movement");
            if (playerMap != null) playerMap.Enable();
        }

        StartCoroutine(HideResultUIAfterDelay());
    }

    private void Lose()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "It escaped :(";

        if (playerMovementScript != null) playerMovementScript.enabled = true;

        if (playerInput != null)
        {
            var playerMap = playerInput.actions.FindActionMap("Movement");
            if (playerMap != null) playerMap.Enable();
        }

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
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPosition += hookPullVelocity;

        if(hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
            hookPullVelocity = 0f;

        if(hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
            hookPullVelocity = 0f;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    void Fish() 
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        // Hacer un vector con 3 tipos de pez, pequeño, medio y grande, cada uno modifica el tiempo de fallo, el hook size, Progress Degradation [...]
    }

    public void StartFishing()
    {
        isFishing = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerInput != null)
        {
            var playerMap = playerInput.actions.FindActionMap("Movement");
            if (playerMap != null)
                playerMap.Disable();
        }

        if (fishingUI != null)
            fishingUI.SetActive(true);
        if (resultUI != null)
            resultUI.SetActive(false);

        hookProgress = 0f;
        failTimer = 25f;

        fishPosition = UnityEngine.Random.Range(0f, 1f);
        fishDestination = fishPosition;
    }

    void ActualizarInsignia()
    {
        if (insigniaImage == null) return;

        if (puntos >= 300) insigniaImage.sprite = insigniaOro;
        else if (puntos == 200) insigniaImage.sprite = insigniaPlata;
        else if (puntos == 100) insigniaImage.sprite = insigniaBronce;
        else insigniaImage.sprite = null;
    }

    void MostrarResultadoFinal()
    {
        isFishing = false;
        fishingUI.SetActive(false);

        string mensaje = puntos switch
        {
            >= 300 => "Wow!",
            200 => "Incredible!",
            100 => "Well done!",
            _ => "Oops :("
        };

       // if (puntos >= 300)
         //   mensaje = "¡Wow!";
       // else if (puntos == 200)
         //   mensaje = "Incredible!";
       // else if (puntos == 100)
         //   mensaje = "Well done!";
       // else
         //   mensaje = "Oops :(";

        if(mensajeFinalText != null)
            mensajeFinalText.text = mensaje;
    }
}
