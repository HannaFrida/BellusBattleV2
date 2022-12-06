using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class SettingsUIHandler : UIMenuHandler
{
    [SerializeField] private AudioMixer am;
    [SerializeField] private Volume globalVolume;
    private static AudioMixer audioMixer;
    //public TMP_Dropdown resolutionDropDown;
    static Resolution[] resolutions;
    TextMeshProUGUI textPro;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SetUpResolution();
        globalVolume = GameObject.FindObjectOfType<Volume>();
        audioMixer = am;
    }
    private void OnLevelWasLoaded(int level)
    {
        globalVolume = GameObject.FindObjectOfType<Volume>();
    }

    public void SetMasterValume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        textPro.text = (sliderValue * 10).ToString("0#");
    }

    public void SetMusicValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
        textPro.text = (sliderValue * 10).ToString("0#");
    }
    public void SetEffectValume(float sliderValue)
    {
        audioMixer.SetFloat("EffectSounds", Mathf.Log10(sliderValue) * 20);
        textPro.text = (sliderValue *10).ToString("0#");
    }
    public void SetValumeText(TextMeshProUGUI textPro)
    {
        this.textPro = textPro;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // double kolla med gruppden hur vårt olika quality är
    }
    public static void SetFullscrean(bool isFullscrean)
    {
        Screen.fullScreen = isFullscrean;
    }
    public void SetBlur(bool toggle)
    {


        VolumeProfile profile = globalVolume.sharedProfile;
        if (!profile.TryGet<DepthOfField>(out var dof))
        {
            dof = profile.Add<DepthOfField>(toggle);
        }
        dof.active = toggle;

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
