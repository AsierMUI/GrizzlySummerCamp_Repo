using UnityEngine;

public class FishingScoreHandler : MonoBehaviour
{
    //Administra  los puntos de el minijuego de pesca unicamente y se los envia a el scoremanager

    [System.Serializable]
   public class FishScore
    {
        public string fishName;
        public int basePoints;
    }

    [Header("Fish Score")]
    public FishScore easyFish;
    public FishScore mediumFish;
    public FishScore hardFish;

    public void OnFishCaptured(FishingSkillCheck.SkillDifficulty difficulty)
     {
         int points = GetPointsByDifficulty(difficulty);

         if (points <= 0)
         {
             Debug.LogWarning($"No hay puntos asignados");
             return;
         }
        
        ScoreManager.Instance.AddPoints(points);

        Debug.Log($"[FishingScoreHandler]+{points} puntos ({difficulty})");
     }

    int GetPointsByDifficulty(FishingSkillCheck.SkillDifficulty difficulty)
    {
        switch (difficulty)
        {
            case FishingSkillCheck.SkillDifficulty.Easy:
                return easyFish.basePoints;

            case FishingSkillCheck.SkillDifficulty.Medium:
                return mediumFish.basePoints;

            case FishingSkillCheck.SkillDifficulty.Hard:
                return hardFish.basePoints;

            default:
                return 0;
        }
    }
}