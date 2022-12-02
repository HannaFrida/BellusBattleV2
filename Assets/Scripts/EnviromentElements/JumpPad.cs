using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float forceAmountY;
    [SerializeField] private float forceAmountX;
    [SerializeField] private VisualEffect effect;
    [SerializeField] private SoundManager SoundManager;
    private Vector2 force;
    // Start is called before the first frame update
    void Start()
    {
        force = new Vector2(forceAmountX, forceAmountY);
        SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            effect.Play();
            SoundManager.TrampolineSound();
            other.gameObject.GetComponent<PlayerMovement>().AddExternalForce(force);
        }
        
    }
    
}
