using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GwynExplode : Grenade
{
    private bool hasExploded;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.Equals("Player"))
        {
            if (!hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
        else
        {

        }
        
    }
}
