using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;

    private Resolution[] allResolutions;
    private List<Resolution> uniqueResolutions = new();

    public int ResolutionIndex { get; private set;}
    public bool IsFullScreen { get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CacheResolutions();
        Load();
        Apply();
    }

    public List<string> GetResolutionOptions() 
    {
        List<string> options = new();
        foreach (var res in uniqueResolutions) 
        {
            options.Add($"{res.width} x {res.height}");
        }
        return options;
    }

    public int GetCurrentResolutionIndex() => ResolutionIndex;
    public bool GetFullscreen() => IsFullScreen;

    public void SetResolution(int index) 
    {
        if (index < 0 || index >= uniqueResolutions.Count)
            return;
        
        ResolutionIndex = index;
        PlayerPrefs.SetInt("ResolutionIndex", ResolutionIndex);
        Apply();
    }
    public void SetFullscreen(bool fullscreen)
    {
        IsFullScreen = fullscreen;
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
        Apply();
    }

    private void CacheResolutions() 
    {
        allResolutions = Screen.resolutions;

        uniqueResolutions.Clear();
        foreach (var res in allResolutions) 
        {
            if (!uniqueResolutions.Exists(r =>
                r.width == res.width && r.height == res.height)) 
            {
                uniqueResolutions.Add(res);
            }
        }
    }

    private void Load() 
    {
        ResolutionIndex = PlayerPrefs.GetInt(
                "ResolutionIndex",
                GetDefaultResolutionIndex()
                );

        IsFullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    private void Apply() 
    {
        Resolution res = uniqueResolutions[ResolutionIndex];
        Screen.SetResolution(res.width, res.height, IsFullScreen);
    }

    private int GetDefaultResolutionIndex() 
    {
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            if (uniqueResolutions[i].width == Screen.currentResolution.width && uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        
        return uniqueResolutions.Count - 1;
    }
    /*
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
    }*/
}
