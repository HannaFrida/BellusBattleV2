using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GwynExplode : Grenade
{
    private bool hasExploded;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
            
     }
        
}

