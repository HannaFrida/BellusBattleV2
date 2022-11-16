using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionManager : MonoBehaviour
{
    [SerializeField] private PoisonZone[] poisionZones;
    [SerializeField] private float poisionDuration;
    [SerializeField] private float waitBetweenPoision;
    [SerializeField, Tooltip("Aktiverar en random poisionzon istället för att aktivera alla")] private bool chooseRandomZone;
    private bool isPoisionActive;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        foreach(PoisonZone poisionZone in poisionZones)
        {
            poisionZone.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if((isPoisionActive == false && timer >= waitBetweenPoision) || (isPoisionActive == true && timer >= poisionDuration))
        {
            TogglePoisionZones();
        }
    }

    private void ToggleAllPoisionZones()
    {
        foreach(PoisonZone poisionZone in poisionZones)
        {
            if(isPoisionActive == false && isPoisionActive == false)
            {
                poisionZone.gameObject.SetActive(true);
            }
            else
            {
                poisionZone.Clear();
                poisionZone.gameObject.SetActive(false);
            }
        }
    }

    private void ToggleRandomPosionZone()
    {
        PoisonZone pZone = poisionZones[Random.Range(0, poisionZones.Length - 1)];
        pZone.gameObject.SetActive(true);
        
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
}
