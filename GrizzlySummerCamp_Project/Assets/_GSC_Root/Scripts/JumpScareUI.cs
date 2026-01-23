using UnityEngine;
using System.Collections;

public class JumpScareUI : MonoBehaviour
{
    public int clicksNecesarios = 5;
    private int contadorClicks = 0;

    public GameObject interfazSecreta;

    [Header("Animator")]
    public Animator animatorJumpscare;

    [Header("Audio")]
    public string keyJumpScareSFX;

    private bool activado = false;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public void BotonPresionado()
    {
        if (activado) return;

        contadorClicks++;

        if (contadorClicks >= clicksNecesarios)
        {
            activado = true;
            StartCoroutine(ActivarEasterEgg());
        }
    }

    IEnumerator ActivarEasterEgg()
    {
        interfazSecreta.SetActive(true);

        if (audioManager != null && !string.IsNullOrEmpty(keyJumpScareSFX))
            audioManager.PlaySFX(keyJumpScareSFX);

        float duracionAnimacion = animatorJumpscare.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(duracionAnimacion);
        Application.Quit();
    }
}