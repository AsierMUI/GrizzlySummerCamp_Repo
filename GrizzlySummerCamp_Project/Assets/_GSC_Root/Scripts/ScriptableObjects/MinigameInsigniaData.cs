using UnityEngine;

[CreateAssetMenu(fileName = "MinigameInsigniaData", menuName = "Minigames/InsigniaData")]
public class MinigameInsigniaData : ScriptableObject
{
    public string minigameSceneName;

    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
}