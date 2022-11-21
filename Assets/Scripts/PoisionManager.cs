using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisionManager : MonoBehaviour
{
    [SerializeField] private PoisonZone[] poisionZones;
    private SoundManager soundManager;
    [SerializeField] private AudioClip posionSound;
    [SerializeField] private Image warningIcon;
    [SerializeField] private float poisionDuration;
    [SerializeField] private float waitBetweenPoision;
    [SerializeField, Tooltip("Aktiverar en random poisionzon istället för att aktivera alla")] private bool chooseRandomZone;
    [SerializeField] private bool isPoisionActive;
    private float timeBeforeHazard = 3f;
    private float timer;
    private PoisonZone chosenZone; // Används bara om chooseRandomZone är aktiverat
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
        timer += Time.deltaTime;
        if(isPoisionActive == false && waitBetweenPoision - timer < timeBeforeHazard)
        {
            DisplayWarning(true);    
        }
        if((isPoisionActive == false && timer >= waitBetweenPoision) || (isPoisionActive == true && timer >= poisionDuration))
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
                
                poisionZone.gameObject.SetActive(true);
                soundManager.FadeInPoisionHazard("poisonRainHazard");
            }
            else
            {
                DisplayWarning(false);
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
        timer = 0;
    }

    private void DisplayWarning(bool toggle)
    {
        warningIcon.enabled = toggle;  
    }
}
