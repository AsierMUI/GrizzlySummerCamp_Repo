using UnityEngine;
using System.Collections.Generic;

public class NPCStateManager : MonoBehaviour
{
    // este scirpt va en rootpersistente
    public static NPCStateManager Instance;

    private Dictionary<string, NPCState> npcStates = new Dictionary<string, NPCState>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public NPCState GetState(string npcID)
    {
        if (string.IsNullOrEmpty(npcID))
        {
            Debug.LogError("[NPCStateManager] npcID vacio");
            return new NPCState();
        }

        if (!npcStates.ContainsKey(npcID))
            npcStates[npcID] = new NPCState();

        return npcStates[npcID];
    }
}

[System.Serializable]
public class NPCState
{
    public bool tutorialCompletado;
    public bool dialogoNormalUsado;
}