using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnDeath : MonoBehaviour
{
    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) //Was Deleted
        {
            transform.parent.GetComponentInChildren<ParticleSystem>().Play();
            SoundManager.Instance.GlassShatterSound();
        } 
    }
}
