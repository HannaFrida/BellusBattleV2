using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoisionManager : MonoBehaviour
{
    [SerializeField] private PoisonZone[] poisionZones;
    [SerializeField] private AudioClip posionSound;
    [SerializeField] private HazardWarner hazardWarner;
    [SerializeField] private float poisionDuration;
    [SerializeField] private float waitBetweenPoision;
    [SerializeField, Tooltip("Aktiverar en random poisionzon istället för att aktivera alla")] private bool chooseRandomZone;

    private SoundManager soundManager;
    private PoisonZone chosenZone;// Används bara om chooseRandomZone är aktiverat

    private bool isPoisionActive;

    private float timeBeforeHazard = 3f;
    private float poisionTimer;
   
    // Start is called before the first frame update
    void Start()
    {
        hazardWarner.DisplayWarning(false);
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        foreach (PoisonZone poisionZone in poisionZones)
        {
            poisionZone.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        poisionTimer += Time.deltaTime;

        if(isPoisionActive == false && waitBetweenPoision - poisionTimer < timeBeforeHazard)
        {
            hazardWarner.DisplayWarning(true);    
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
                hazardWarner.DisplayWarning(false);
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
   
}
