using UnityEngine;
using TMPro;

public class TrashCarryUI : MonoBehaviour
{
    [Tooltip("Texto para mostrar que sí se lleva basura")]
    [SerializeField] TrashPlayerCarry carry;
    [Tooltip("NO cé")]
    [SerializeField] TMP_Text carryText;
    [Tooltip("Icono para indicar que lleva basura")]
    [SerializeField] GameObject carryIcon;

    //Hacer un controlador de cuantos items tienes.

    private void Update()
    {
        if (carry == null) return;

        bool carrying = carry.IsCarryingTrash();

        carryIcon.SetActive(carrying);
        carryText.text = carrying ? "1" : "0";
    }
}
