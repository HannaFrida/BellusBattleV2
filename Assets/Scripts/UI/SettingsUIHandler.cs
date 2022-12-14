using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class SettingsUIHandler : UIMenuHandler
{
    [SerializeField] private GameObject bvs;
    [SerializeField] private GameObject bas;
    [SerializeField] private GameObject bgs;
    [SerializeField] private AudioMixer am;
    [SerializeField] private ControlChooser cc;
    [SerializeField] private Volume globalVolume;
    private static AudioMixer audioMixer;
    private SoundManager sm;
    //public TMP_Dropdown resolutionDropDown;
    static Resolution[] resolutions;
    TextMeshProUGUI textPro;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SetUpResolution();
        cc = GetComponent<ControlChooser>();
        sm = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        globalVolume = GameObject.FindObjectOfType<Volume>();
        audioMixer = am;
        es.SetSelectedGameObject(bvs);
    }
    private void OnLevelWasLoaded(int level)
    {
        globalVolume = GameObject.FindObjectOfType<Volume>();
    }
    private void OnEnable()
    {
        switch (activePanel.name)
        {
            case "VisualsSelectedButtons": es.SetSelectedGameObject(bvs); break;
            case "AudioSelectedButtons": es.SetSelectedGameObject(bas); break;
            case "GameplaySelectedButtons": es.SetSelectedGameObject(bgs); break;
            default: es.SetSelectedGameObject(bvs); break;
        }
    }
    override public void SetPanelActive(GameObject panel)
    {
        panel.SetActive(true);
        activePanel.SetActive(false);
        activePanel = panel;
        switch (panel.name)
        {
            case "VisualsSelectedButtons": es.SetSelectedGameObject(bvs); break;
            case "AudioSelectedButtons": es.SetSelectedGameObject(bas); break;
            case "GameplaySelectedButtons": es.SetSelectedGameObject(bgs); break;
        }
    }

    public void SetMasterValume(float sliderValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        
        textPro.text = (sliderValue * 10).ToString("0#");
    }

    public void SetMusicValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
        sm.SetHighestMusicVolume(sliderValue);
        textPro.text = (sliderValue * 10).ToString("0#");
    }
    public void SetEffectValume(float sliderValue)
    {
        audioMixer.SetFloat("EffectSounds", Mathf.Log10(sliderValue) * 20);
        sm.SetHighestEffectVolume(sliderValue);
        textPro.text = (sliderValue *10).ToString("0#");
    }
    public void SetText(TextMeshProUGUI textPro)
    {
        this.textPro = textPro;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // double kolla med gruppden hur v?rt olika quality ?r
    }
    public void SetOneHandMode(bool toggle)
    {
        if (toggle) textPro.text = "ON";
        else textPro.text = "OFF";
        cc.LeftControllerMode();
        
    }
    public void SetFullscrean(bool isFullscrean)
    {
        Screen.fullScreen = isFullscrean;
        if(isFullscrean)textPro.text = "ON";
        else textPro.text = "OFF";
    }
    public void SetBlur(bool toggle)
    {
        if (toggle) textPro.text = "ON";
        else textPro.text = "OFF";

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
    public void SetUIInicator(bool isFullscrean)
    {

    }

    public void SetSlowMoText(float sliderValue)
    {
        textPro.text = (sliderValue).ToString("#")+ "0%";
        if (textPro.text.Equals("100%")) textPro.text = "Normal speed";
    }
    override
    public void ExitUI()
    {
        base.ExitUI();

    }
}
