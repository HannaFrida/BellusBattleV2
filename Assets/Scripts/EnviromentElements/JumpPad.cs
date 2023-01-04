using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float forceAmountY;
    [SerializeField] private float forceAmountX;
    [SerializeField] private VisualEffect effect;
    private Vector2 force;
    // Start is called before the first frame update
    void Start()
    {
        force = new Vector2(forceAmountX, forceAmountY);
    }

    private void OnTriggerEnter(Collider other)
    {
        AddForceToObject(other.gameObject, other.tag);  
    }

    private void AddForceToObject(GameObject go, string tag)
    {
        if (tag.Equals("Player") == false && tag.Equals("Grenade") == false) return;
        effect.Play();
        SoundManager.Instance.TrampolineSound();

        if (tag.Equals("Player"))
        {
            go.GetComponent<PlayerMovement>().AddExternalForce(force);
        }
        else
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(force * rb.mass, ForceMode.Impulse);
        }


    }
    
}
