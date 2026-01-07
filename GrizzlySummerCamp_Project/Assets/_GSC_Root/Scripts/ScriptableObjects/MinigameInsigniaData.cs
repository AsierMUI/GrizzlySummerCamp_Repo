using UnityEngine;

[CreateAssetMenu(fileName = "MinigameInsigniaData", menuName = "Minigames/InsigniaData")]
public class MinigameInsigniaData : ScriptableObject
{
    [Header("Minigame")]
    public string minigameSceneName;

    [Header("Insignias")]
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;

    public Sprite GetSpriteByInsignia(int insignia)
    {
        switch (insignia)
        {
            case 3: return gold;
            case 2: return silver;
            case 1: return bronze;
            default: return null;
        }
    }

}