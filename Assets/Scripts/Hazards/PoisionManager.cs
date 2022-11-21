using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisionManager : MonoBehaviour
{
    [SerializeField] private PoisonZone[] poisionZones;
    [SerializeField] private AudioClip posionSound;
    [SerializeField] private Image warningIcon;
    [SerializeField] private float poisionDuration;
    [SerializeField] private float waitBetweenPoision;
    [SerializeField, Tooltip("Aktiverar en random poisionzon istället för att aktivera alla")] private bool chooseRandomZone;

    private SoundManager soundManager;
    private PoisonZone chosenZone;

    private bool isShowingWarning;// Används bara om chooseRandomZone är aktiverat
    private bool isPoisionActive;

    private float timeBeforeHazard = 3f;
    private float blinkTimer, poisionTimer;
    private float blinkTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        DisplayWarning(false);
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        foreach (PoisonZone poisionZone in poisionZones)
        {
            poisionZone.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isShowingWarning == true)
        {
            RunBlinkTimer();
        }
        poisionTimer += Time.deltaTime;

        if(isPoisionActive == false && waitBetweenPoision - poisionTimer < timeBeforeHazard)
        {
            DisplayWarning(true);    
        }
        if((isPoisionActive == false && poisionTimer >= waitBetweenPoision) || (isPoisionActive == true && poisionTimer >= poisionDuration))
        {
            TogglePoisionZones();
        }
    }

    private void ToggleAllPoisionZones()
    {
        foreach(PoisonZone poisionZone in poisionZones)
        {
            if(isPoisionActive == false)
            {
                DisplayWarning(false);
                poisionZone.gameObject.SetActive(true);
                soundManager.FadeInPoisionHazard("poisonRainHazard");
            }
            else
            {
                poisionZone.Clear();
                poisionZone.gameObject.SetActive(false);
                soundManager.FadeOutHazard();
            }
            
        }
    }
    private void ToggleRandomPosionZone()
    {
        
        if (isPoisionActive == false)
        {
            chosenZone = poisionZones[Random.Range(0, poisionZones.Length - 1)];
            chosenZone.gameObject.SetActive(true);
        }
        else
        {
            chosenZone.gameObject.SetActive(false);
        }  
    }

    private void TogglePoisionZones()
    {
        if(chooseRandomZone == true)
        {
            ToggleRandomPosionZone();
        }
        else
        {
            ToggleAllPoisionZones();
        }
        isPoisionActive = !isPoisionActive;
        poisionTimer = 0;
    }

    private void DisplayWarning(bool toggle)
    {
        isShowingWarning = toggle;
        warningIcon.enabled = toggle;  
    }

    private void WarningBlink()
    {
        switch (warningIcon.color.a.ToString())
        {
            case "1":
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 0);
                break;
            case "0":
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 1);
                break;
        }
        blinkTimer = 0f;
    }

    private void RunBlinkTimer()
    {
        blinkTimer += Time.deltaTime;

        if(blinkTimer >= blinkTime)
        {
            WarningBlink();
        }
    }
}
