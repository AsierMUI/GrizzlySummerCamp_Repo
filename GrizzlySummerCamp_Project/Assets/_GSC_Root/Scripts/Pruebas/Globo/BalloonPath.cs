using UnityEngine;
using System.Collections.Generic;

public class BalloonPath : MonoBehaviour
{
    public Transform spawnPoint;
    public List<BalloonRoute> routes = new List<BalloonRoute>();

    public Transform[] GetRandomRoute()
    {
        if (routes.Count == 0)
            return null;

        return routes[Random.Range(0, routes.Count)].waypoints;
    }
}