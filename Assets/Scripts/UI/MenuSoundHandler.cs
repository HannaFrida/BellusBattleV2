using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
* Author Simon Hessling
*/
public class MenuSoundHandler : MonoBehaviour
{
    private SoundManager soundManager;
    private EventSystem eventSystem;
    private GameObject currentSelectedUiItem;
    
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        soundManager = SoundManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        HowerSoundPlayer();
    }
    private void HowerSoundPlayer()
    {
        if(currentSelectedUiItem != eventSystem.currentSelectedGameObject)
        {
            
            soundManager.HowerUiSound();
            currentSelectedUiItem = eventSystem.currentSelectedGameObject;
        }
        
    }
    private void PressSoundPlayer()
    {
        soundManager.PressUiSound();
    }
}
