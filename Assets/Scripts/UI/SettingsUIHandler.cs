using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SettingsUIHandler : UIMenuHandler
{
    [SerializeField] private AudioMixer am;
    private static AudioMixer audioMixer;
    //public TMP_Dropdown resolutionDropDown;
    static Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SetUpResolution();
        audioMixer = am;
    }

    public void SetMasterValume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }
    public static void SetMusicValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
    }
    public static void SetEffectValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // double kolla med gruppden hur vårt olika quality är
    }
    public static void SetFullscrean(bool isFullscrean)
    {
        Screen.fullScreen = isFullscrean;
    }
    private void SetUpResolution()
    {
        resolutions = Screen.resolutions;
        //resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //resolutionDropDown.AddOptions(options);
        //resolutionDropDown.value = currentResolutionIndex;
        //resolutionDropDown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
