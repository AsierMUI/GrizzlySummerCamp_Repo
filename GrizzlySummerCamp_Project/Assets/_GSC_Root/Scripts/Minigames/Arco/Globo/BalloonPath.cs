using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BalloonPath
{
    public Transform spawnPoint;
    public List<BalloonRoute> routes = new List<BalloonRoute>();
}