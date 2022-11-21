using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GrenadeShell : Grenade
{
    private bool hasExploded;
    private void OnCollisionEnter(Collision collision)
    {
        if(!hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }
}
