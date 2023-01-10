using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
* Author Hanna Rudöfors
*/
public class PlayerDetails : MonoBehaviour
{
    public int playerID; // Stores the players ID
    public Vector3 startPos; // Stores the start spawn position
    public bool isAlive;
    [SerializeField] private Transform headGearSlot;
    [SerializeField] private InputDevice device;
    private bool isRumbling;
    private float rumbleTime = 0.3f;
    private float timer;
    private string chosenActionMap = "Player";

    public string ChosenActionMap
    {
        get => chosenActionMap;
        set => chosenActionMap = value;
    }

    public Transform HeadGearSlot()
    {
        return headGearSlot;
    }
    
    void Start()
    {
        transform.position = startPos; // Puts the player on the spawn position
        isAlive = true;
    }


    private void Update()
    {
        RunRumbleTimer();
    }

    public void OnPlayerPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.GameIsPaused == false)
            {
                GameManager.Instance.PauseGame();
            }
            else
            {
                GameManager.Instance.ResumeGame();
                
            }
        }
        
    }

    public void SetDevice(InputDevice device)
    {
        this.device = device;
    }
    /*
* Author Martin Wallmark
*/
    public void Rumble(float lowF, float highF)
    {
        isRumbling = true;
        Gamepad gamepad = (Gamepad)device;
        gamepad.SetMotorSpeeds(lowF, highF);
    }

    private void RunRumbleTimer()
    {
        if (isRumbling == false) return;

        timer += Time.deltaTime;
        if(timer >= rumbleTime)
        {
            Rumble(0, 0);
            isRumbling = false;
            timer = 0f;
        }
    }
}
