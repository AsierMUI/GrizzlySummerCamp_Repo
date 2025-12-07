using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Pivots")]
    public Transform topPivot;
    public Transform bottomPivot;

    [Header("Fish Stats")]
    public Transform fish;
    public float fishPosition;
    public float fishDestination;
    public float fishTimer;
    public float timerMultiplicator = 3f;
    public float fishSpeed;
    public float smoothMotion = 1f;

    public enum Dificultad { Facil, Normal, Dificil }
    public Dificultad dificultadActual;

    private void Start()
    {
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;
    }

    public void Tick()
    {
        fishTimer -= Time.deltaTime;

        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp( fishPosition, fishDestination, ref fishSpeed, smoothMotion );

        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
}
