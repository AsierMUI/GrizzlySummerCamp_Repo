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
    [SerializeField] private GameObject canvasInstrucciones;
    [SerializeField] private GameObject LoseUI;
    [SerializeField] private GameObject temporizadorUI;
    [SerializeField] private GameObject WinUI;

    private bool temporizadorActivo = false;

    private void Start()
    {
        tiempoRestante = tiempoMax;
        slider.maxValue = tiempoMax;
        slider.value = tiempoRestante;
        LoseUI.SetActive(false);
        canvasInstrucciones.SetActive(true);
        temporizadorUI.SetActive(false);
        WinUI.SetActive(false);
    }

    private void Update()
    {
        if (!temporizadorActivo && canvasInstrucciones !=null && !canvasInstrucciones.activeSelf)
        {
            temporizadorActivo = true;
            temporizadorUI.SetActive(true);
        }

        if (!temporizadorActivo) return;

        if (WinUI != null && WinUI.activeSelf)
            return;

        tiempoRestante -= Time.deltaTime;
        slider.value = Mathf.Clamp(tiempoRestante, 0, tiempoMax);

        if(tiempoRestante <= 0)
        {
            temporizadorActivo = false;
            LoseUI.SetActive(true);
        }

    }
}
