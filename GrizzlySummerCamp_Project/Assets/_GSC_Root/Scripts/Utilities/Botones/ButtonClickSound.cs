using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{
    public AudioManager audioManager;


    [System.Serializable]
    public class ButtonSound 
    {
        public Button button;
        public string soundKey;
    
    }


    [System.Serializable]
    public class ButtonSoundGroup
    {
        public string groupName = "Nuevo grupo";
        public List<ButtonSound> sounds = new();
        public bool foldout = true; // Estado visual de expandido en inspector
    }


    public List<ButtonSoundGroup> groups = new();

    private void Start()
    {
        foreach (var group in groups)
        {
            foreach (var bs in group.sounds)
            {
                if (bs != null && bs.button != null)
                {
                    bs.button.onClick.AddListener(() => audioManager.PlaySFX(bs.soundKey));
                }
            }
        }
    }

}
