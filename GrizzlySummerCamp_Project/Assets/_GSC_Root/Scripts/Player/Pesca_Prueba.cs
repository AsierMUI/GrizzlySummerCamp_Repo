using UnityEngine;

public class Pesca_Prueba : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    //This won't do for now
    //[SerializeField] Transform leftPivot;
    //[SerializeField] Transform rightPivot;

    //Will need to add fish stats

    float fishPosition;
    float fishDestination;


    [SerializeField] float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;

    [SerializeField] float fishSpeed;
    [SerializeField] float smoothMotion = 1f;



    void Start()
    {
        
    }
    void Update()
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
}
