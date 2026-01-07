using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Minigames/Insignia Database")]
public class InsigniaDataBase : ScriptableObject
{
    public List<MinigameInsigniaData> minigames;

    public MinigameInsigniaData GetByScene(string sceneName)
    {
        return minigames.Find(minigames => minigames.minigameSceneName == sceneName);
    }
}