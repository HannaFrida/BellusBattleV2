using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlChooser : MonoBehaviour
{
    [SerializeField] private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private static bool toggledLeft;
    private static bool toggledRight;


    private void Update()
    {
        foreach(PlayerInput input in GameManager.Instance.GetInputs())
        {
            //Debug.Log("wjaja");
            if (!playerInputs.Contains(input))
            {
                playerInputs.Add(input);
            }
            
        }
    }

    public void LeftControllerMode()
    {
        toggledLeft = !toggledLeft;
        toggledRight = false;
        ControlScheme();


    }
    public void RightControllerMode()
    {
        toggledRight = !toggledRight;
        toggledLeft = false;
        ControlScheme();
    }

    public void AddToList(PlayerInput input)
    {
        playerInputs.Add(input);
    }


    void ControlScheme()
    {
        if (toggledLeft)
        {           
            foreach (PlayerInput input in playerInputs)
            {
                input.SwitchCurrentActionMap("PlayerAccessibilityLeft");
                input.gameObject.GetComponent<PlayerMovement>().HasAccessibility = true;
                input.gameObject.GetComponent<PlayerDetails>().ChosenActionMap = "PlayerAccessibilityLeft";
            }
            
            Debug.Log("s�tt in left controller control scheme h�r");
        }
        if (toggledRight)
        {
            Debug.Log("s�tt in right controller control scheme h�r");
            foreach (PlayerInput input in playerInputs)
            {
                input.SwitchCurrentActionMap("PlayerAccessibilityRight");
            }

        }
        if (!toggledRight && !toggledLeft)
        {
            Debug.Log("S�tt in normal kotroller h�r");
            foreach (PlayerInput input in playerInputs)
            {
                input.SwitchCurrentActionMap("Player");
                input.gameObject.GetComponent<PlayerDetails>().ChosenActionMap = "Player";
                input.gameObject.GetComponent<PlayerMovement>().HasAccessibility = false;
            }

        }
    }
}
