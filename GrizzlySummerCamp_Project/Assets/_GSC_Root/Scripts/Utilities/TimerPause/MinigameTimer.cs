using UnityEngine;
using System;

//SIRVE PARA TODOS LOS MINIJUEGOS

public class MinigameTimer : MonoBehaviour
{
    //Parece no estar en uso.


    public static MinigameTimer instance;

    [Header("Timer Settings")]
    [SerializeField] float totalTime = 90f;

    public float CurrentTime {  get; private set; }

    public bool IsRunning { get; private set; }

    public event Action OnTimerFinished;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        ResetTimer();
        PauseTimer();
    }

    private void Update()
    {
        if (!IsRunning) return;

        CurrentTime -= Time.deltaTime;

        if (CurrentTime <= 0f)
        {
            CurrentTime = 0f;
            IsRunning = false;
            OnTimerFinished?.Invoke();
        }
    }

    public void StartTimer()
    {
        IsRunning = true;
    }

    public void PauseTimer()
    {
        IsRunning = false;
    }

    public void ResetTimer()
    {
        CurrentTime = totalTime;
    }
}