using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/*
* Author Martin Wallmark
*/
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button firstSelected;
    [SerializeField] private Button quitCanvasFirstSelected, lobbyCanvasFirstSelected;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject firstSelectedOnSettings;
    [SerializeField] private GameObject quitCanvas, lobbyCanvas;
    private bool isToggled;
    private GameManager gm;
    private EventSystem es;
    private SoundManager sm;
    private bool runButtonCheck;
    private bool isQuitCanvasToggled;
    private bool isLobbyCanvasToggled;
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
        CheckSelectedButton();
        if(quitCanvas.activeSelf == true && isQuitCanvasToggled == false)
        {
            es.SetSelectedGameObject(quitCanvasFirstSelected.gameObject);
            isQuitCanvasToggled = true;
        }
        if (lobbyCanvas.activeSelf == true && isLobbyCanvasToggled == false)
        {
            es.SetSelectedGameObject(lobbyCanvasFirstSelected.gameObject);
            isLobbyCanvasToggled = true;
        }
    }

    private void TogglePauseMenu()
    {
        if (gm.GameIsPaused == true && isToggled == false)
        {
            sm.HalfMusicVolume();
            //Debug.Log("dada");
            isToggled = true;
            DisplayPauseMenu(isToggled);
        }
        else if(gm.GameIsPaused == false && isToggled == true)
        {
            sm.FullMusicVolume();
            isToggled = false;
            isQuitCanvasToggled = false;
            quitCanvas.SetActive(false);
            isLobbyCanvasToggled = false;
            lobbyCanvas.SetActive(false);
            DisplayPauseMenu(isToggled);
        }
        
    }

    public void ResetSelectedButton()
    {
        isQuitCanvasToggled = false;
        isLobbyCanvasToggled = false;
        es.SetSelectedGameObject(firstSelected.gameObject);
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
        GameDataTracker.Instance.ClearSavedData();
        gm.ReturnToMainMenu();
    }

    public void QuitGame()
    {
        GameDataTracker.Instance.WriteToFile();
        Application.Quit();
    }

    private void CheckSelectedButton()
    {
        if (runButtonCheck == false) return;

        if(es.firstSelectedGameObject != firstSelected)
        {
            es.SetSelectedGameObject(firstSelected.gameObject);
            runButtonCheck = false;
        }
    }

}
