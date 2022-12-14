using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnDeath : MonoBehaviour
{
    private void OnDestroy()
    {
        transform.parent.GetComponentInChildren<ParticleSystem>().Play();
    }



}
