using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource doubleJumpSound;
    [SerializeField] private AudioSource landSound;
    [SerializeField] private AudioSource dashSound;
    private float lowPitchRan = 0.93f;
    private float highPitchRan = 1.0f;
    // Start is called before the first frame update
    public void PlayerJumpSound()
    {
        jumpSound.pitch = Random.Range(lowPitchRan, highPitchRan);
        jumpSound.Play();
    }
    public void PlayerDoubleJumpSound()
    {
        jumpSound.pitch = Random.Range(lowPitchRan, highPitchRan);
        jumpSound.Play();
    }
    public void PlayerLandSound()
    {
        landSound.pitch = Random.Range(lowPitchRan, highPitchRan);
        landSound.Play();
    }
    public void PlayerDashSound()
    {
        dashSound.pitch = Random.Range(lowPitchRan, highPitchRan);
        dashSound.Play();
    }
}
