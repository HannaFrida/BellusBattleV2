using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsUIHandler : UIMenuHandler
{
    [SerializeField] private AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public void SetMasterValume(float sliderValue)
    {
        audioMixer.SetFloat("MasterValume", Mathf.Log10(sliderValue) * 20);
    }
    public void SetMusicValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // double kolla med gruppden hur v�rt olika quality �r
    }
    public void SetFullscrean(bool isFullscrean)
    {
        Screen.fullScreen = isFullscrean;
    }
}
