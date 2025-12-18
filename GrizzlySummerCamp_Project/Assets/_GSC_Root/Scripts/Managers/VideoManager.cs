using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoManager : MonoBehaviour
{
    public TMP_Dropdown ResDropDown;
    public Toggle FullScreenToggle;

    Resolution[] AllResolutions;
    bool IsFullScreen;
    int SelectedResolution;
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    private void Start()
    {
        IsFullScreen = true;
        AllResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution res in AllResolutions) 
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if (!resolutionStringList.Contains(newRes))
            { 
                resolutionStringList.Add(newRes);
                SelectedResolutionList.Add(res);
            }
        }

        ResDropDown.AddOptions(resolutionStringList);
    }

    public void ChangeResolution() 
    {
        SelectedResolution = ResDropDown.value;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, IsFullScreen);
        Debug.Log("Resolución cambiada " + Screen.currentResolution);
    }

    public void ChangeFullScreen() 
    {
        IsFullScreen = FullScreenToggle.isOn;
        Screen.SetResolution(SelectedResolutionList[SelectedResolution].width, SelectedResolutionList[SelectedResolution].height, IsFullScreen);
    }
}
