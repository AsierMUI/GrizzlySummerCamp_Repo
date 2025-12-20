using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static Vector3 lastPosition = Vector3.zero;
    public static bool hasSavedPosition = false;

    public static void SavePosition(Vector3 pos)
    {
        lastPosition = pos;
        hasSavedPosition = true;
        Debug.Log($"[PlayerData] Posicion guardada: {lastPosition}");
    }
}