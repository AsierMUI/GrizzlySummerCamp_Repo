using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{

    [System.Serializable]
    public class ButtonSound 
    {
        public Button button;
        public string soundKey;
    
    }
  
    public List<ButtonSound> buttonsounds = new();
    private void Start()
    {
        foreach (var bs in buttonsounds) 
        {
            if (bs!=null)
            {
                bs.button.onClick.AddListener(() => AudioManager.Current?.PlaySFX(bs.soundKey));
            }
        }
    }

}
