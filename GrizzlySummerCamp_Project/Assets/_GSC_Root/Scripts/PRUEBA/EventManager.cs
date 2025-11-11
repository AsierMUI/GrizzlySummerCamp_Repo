using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    //maneja los eventos de fishcaught y escaped
    public static Action<Dificultad> OnFishCaught;
    public static Action OnFishEscaped;
}
