using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button firstSelected;
    private bool isToggled;
    private GameManager gm;
    private EventSystem es;
    private SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        es = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        gm = GameManager.Instance;
        DisplayPauseMenu(false);
    }

    // Update is called once per frame
    void Update()
    {
        TogglePauseMenu();
    }

    private void TogglePauseMenu()
    {
        if (gm.GameIsPaused == true && isToggled == false)
        {
            sm.HalfMusicVolume();
            Debug.Log("dada");
            isToggled = true;
            DisplayPauseMenu(isToggled);
        }
        else if(gm.GameIsPaused == false && isToggled == true)
        {
            sm.FullMusicVolume();
            isToggled = false;
            DisplayPauseMenu(isToggled);
            Debug.Log("dada2");
        }
        
    }

    private void DisplayPauseMenu(bool toggle)
    {
        pauseMenuPanel.SetActive(toggle);

        if(toggle == true)
        {
            es.SetSelectedGameObject(firstSelected.gameObject);
        }
    }

    public void Resume()
    {
        gm.ResumeGame();
        DisplayPauseMenu(false);
    }

    public void ReturnToLobby()
    {
        DisplayPauseMenu(false);
        gm.ResumeGame();
        gm.ReturnToMainMenu();
    }
}