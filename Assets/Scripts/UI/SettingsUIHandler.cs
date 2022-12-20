using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using static PlayerMovement;

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
    List<GameObject> players;

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
        players = new List<GameObject>(GameManager.Instance.GetAllPlayers());
    }
    private void OnLevelWasLoaded(int level)
    {
        globalVolume = GameObject.FindObjectOfType<Volume>();
    }
    private void OnEnable()
    {
        if (activePanel == null) return;
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
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        
        textPro.text = (sliderValue * 10).ToString("0#");
    }
    public void GetMasterValume()
    {
        float value = PlayerPrefs.GetFloat("MasterVolume");
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        
        
        textPro.text = (value * 10).ToString("0#");
    }

    public void SetMusicValume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMixerGroup", Mathf.Log10(sliderValue) * 20);
        sm.SetHighestMusicVolume(sliderValue);
        textPro.text = (sliderValue * 10).ToString("0#");
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
    public void SetEffectValume(float sliderValue)
    {
        audioMixer.SetFloat("EffectSounds", Mathf.Log10(sliderValue) * 20);
        sm.SetHighestEffectVolume(sliderValue);
        textPro.text = (sliderValue *10).ToString("0#");
        PlayerPrefs.SetFloat("EffectVolume", sliderValue);
    }
    public void SetText(TextMeshProUGUI textPro)
    {
        this.textPro = textPro;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); // double kolla med gruppden hur v�rt olika quality �r
        PlayerPrefs.SetInt("QualitySetting", qualityIndex);
    }
    public void SetOneHandMode(bool toggle)
    {
        if (toggle) textPro.text = "ON";
        else textPro.text = "OFF";
        cc.LeftControllerMode();
        int boolean = Convert.ToInt32(toggle);
        PlayerPrefs.SetInt("OneHandMode", boolean);
        
    }
    public void SetFullscrean(bool isFullscrean)
    {
        Screen.fullScreen = isFullscrean;
        if(isFullscrean)textPro.text = "ON";
        else textPro.text = "OFF";

        int boolean = Convert.ToInt32(isFullscrean);
        PlayerPrefs.SetInt("FullScreenSetting", boolean);
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
        int boolean = Convert.ToInt32(toggle);
        PlayerPrefs.SetInt("BlurSetting", boolean);

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

    public void SetSlowMoText(float sliderValue)
    {
        textPro.text = (sliderValue).ToString("#")+ "0%";
        if (textPro.text.Equals("100%")) textPro.text = "Normal speed";
        PlayerPrefs.SetFloat("SlowMoValue", sliderValue);
    }
    public void SetAutoJump(int index)
    {
        switch (index)
        {
            case 0:
                foreach (GameObject player in players)
                {
                    if (player != null) player.GetComponent<PlayerMovement>().SetJumpSetting(JumpSetting.Press);
                }
                break;
            case 1:
                foreach (GameObject player in players)
                {
                    if (player != null) player.GetComponent<PlayerMovement>().SetJumpSetting(JumpSetting.Hold);
                }
                break;
            case 2:
                foreach (GameObject player in players)
                {
                    if (player != null) player.GetComponent<PlayerMovement>().SetJumpSetting(JumpSetting.Toggle);
                }
                break;
            default: break;
        }
        PlayerPrefs.SetInt("AutoJumpSetting", index);
    }

    public void SetSafeMode(bool toggle)
    {
        if (toggle) textPro.text = "ON";
        else textPro.text = "OFF";
        GameManager.Instance._safeMode= toggle;
        int boolean = Convert.ToInt32(toggle);
        PlayerPrefs.SetInt("SafeModeSetting", boolean);

    }

    public void SetUIIndecator(bool toggle)
    {
        if (toggle) textPro.text = "ON";
        else textPro.text = "OFF";
        foreach (GameObject player in players)
        {
            if (player != null) player.transform.Find("PlayerIndicator").gameObject.SetActive(toggle);
        }
        int boolean = Convert.ToInt32(toggle);
        PlayerPrefs.SetInt("UIindicatorSetting", boolean);

    }
    override
    public void ExitUI()
    {
        PlayerPrefs.Save();
        base.ExitUI();

    }
}
