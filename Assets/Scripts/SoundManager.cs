using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TextMeshProUGUI valumeValue;
    private void Start()
    {
        valumeValue.text = "10"; // place holder
    }

    public void SetValume(float sliderValue)
    {
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        valumeValue.text = Mathf.Round(sliderValue * 10).ToString();
    }
    
    
}
