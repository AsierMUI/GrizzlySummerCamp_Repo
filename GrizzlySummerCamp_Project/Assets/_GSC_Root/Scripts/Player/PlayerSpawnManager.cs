using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerData.hasSavedPosition)
        {
            transform.position = PlayerData.lastPosition;
        }
    }
}
