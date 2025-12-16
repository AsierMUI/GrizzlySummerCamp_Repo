using UnityEngine;

public class FishingScoreHandler : MonoBehaviour
{
    //Administra  los puntos de el minijuego de pesca unicamente y se los envia a el scoremanager

    [System.Serializable]
   public class FishScoreData
    {
        public string fishID;
        public int baseScore;
    }

    [Header("Fish Score Settings")]
    [SerializeField] private FishScoreData[] fishScores;

    [Header("Difficulty Multipliers")]
    [SerializeField] private float easyMultiplier = 1f;
    [SerializeField] private float mediumMultiplier = 1.5f;
    [SerializeField] private float hardMultiplier = 2f;

    /* public void OnFishCaptured(string fishID, FishingSkillCheck.SkillDifficulty difficulty)
     {
         int baseScore = GetBaseScoreForFish(fishID);
         float multiplier = GetMultiplier(difficulty);

         if (baseScore <= 0)
         {
             Debug.LogWarning($"No hay puntos para el pez : {fishID}");
             return;
         }
     }

     private int GetScoreForFish(string fishID)
     {
         foreach (var fish in fishScores)
         {
             if (fish.fishID == fishID)
                 return fish.scoreValue;
         }

         Debug.LogWarning($"No hay puntos definidos para el pez: {fishID}");
         return 0;
     }*/
}