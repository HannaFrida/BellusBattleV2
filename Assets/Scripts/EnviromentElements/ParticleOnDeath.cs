using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnDeath : MonoBehaviour
{
    private SoundManager sM;
    private void Start()
    {
        sM = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }
    private void OnDestroy()
    {
        transform.parent.GetComponentInChildren<ParticleSystem>().Play();
        sM.GlassShatterSound();
    }



}
