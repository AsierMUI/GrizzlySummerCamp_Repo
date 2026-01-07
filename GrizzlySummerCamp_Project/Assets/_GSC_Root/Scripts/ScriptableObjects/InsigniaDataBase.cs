using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InsigniaDatabase", menuName = "Minigames/Insignia Database")]
public class InsigniaDatabase : ScriptableObject
{
    public List<MinigameInsigniaData> minigames;

    public MinigameInsigniaData GetByScene(string sceneName)
    {
        return minigames.Find(minigames => minigames.minigameSceneName == sceneName);
    }
}