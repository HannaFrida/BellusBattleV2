using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isToggled;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
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
            isToggled = true;
            DisplayPauseMenu(isToggled);
        }
        else if(gm.GameIsPaused == false && isToggled == true)
        {
            isToggled = false;
            DisplayPauseMenu(isToggled);
        }
        
    }

    private void DisplayPauseMenu(bool toggle)
    {
        pauseMenuPanel.SetActive(toggle);
    }
}
