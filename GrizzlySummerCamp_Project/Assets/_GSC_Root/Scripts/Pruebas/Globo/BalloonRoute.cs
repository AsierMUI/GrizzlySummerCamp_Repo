using UnityEngine;

public class BalloonRoute : MonoBehaviour
{
    public Transform[] waypoints;

    private void OnValidate()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            waypoints[i] = transform.GetChild(i);
    }
}