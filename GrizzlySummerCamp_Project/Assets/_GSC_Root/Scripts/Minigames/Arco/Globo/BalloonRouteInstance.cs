using UnityEngine;

[System.Serializable]
public class BalloonRouteInstance
{
    public BalloonPath path;
    public BalloonRoute route;

    public BalloonRouteInstance(BalloonPath p, BalloonRoute r)
    {
        path = p;
        route = r;
    }
}