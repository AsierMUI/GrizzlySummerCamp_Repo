using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempLaberinto : MonoBehaviour
{
    [Header("Tiempo")]
    [SerializeField] private float tiempoMax = 120f;
    private float tiempoRestante;

    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject LoseUI;

    private bool temporizadorActivo = true;

    private void Start()
    {
        tiempoRestante = tiempoMax;
        slider.maxValue = tiempoMax;
        slider.value = tiempoRestante;
        LoseUI.SetActive(false);
    }

    private void Update()
    {
        if (!temporizadorActivo) return;

        tiempoRestante -= Time.deltaTime;
        slider.value = Mathf.Clamp(tiempoRestante, 0, tiempoMax);

        if(tiempoRestante <= 0)
        {
            temporizadorActivo = false;
            LoseUI.SetActive(true);
        }

    }
}
