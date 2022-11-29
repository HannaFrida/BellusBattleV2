using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public Transform HeadGearSlot()
    {
        return headGearSlot;
    }
    
   
    void Start()
    {
        transform.position = startPos; // Puts the player on the spawn position
        isAlive = true;
        //headGearSlot = GameObject.FindGameObjectWithTag("HeadSlot").transform;
    }


    private void Update()
    {
        RunRumbleTimer();
    }

    public void OnPlayerPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Debug.Log("joppa");
            if (GameManager.Instance.GameIsPaused == false)
            {
                GameManager.Instance.PauseGame();
                Debug.Log("jii");
            }
            else
            {
                GameManager.Instance.ResumeGame();
                Debug.Log("hAAA");
            }
        }
        
    }

    public void SetDevice(InputDevice device)
    {
        this.device = device;
    }

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
