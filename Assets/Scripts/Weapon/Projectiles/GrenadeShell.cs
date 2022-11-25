using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GrenadeShell : Grenade
{
    private bool hasExploded;
    private bool hasHitGround;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hasHitGround = true;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasExploded && !hasHitGround)
            {
                Explode();
                hasExploded = true;
            }
        }
        
    }
}
