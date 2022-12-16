using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GrenadeShell : Grenade
{
    private float energyMuliplierOnBounce = 0.5f;
    //private bool hasExploded;
    private bool hasHitGround;
    private Rigidbody rigidBody;
    private Vector3 velocityOnImpact;

    protected override void Start()
    {
        base.Start();
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            velocityOnImpact = collision.relativeVelocity;
            hasHitGround = true;
            Vector3 normal = collision.GetContact(0).normal;
            BounceGrenade(normal);
            
        }
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerHead"))
        {
            if (!hasExploded && !hasHitGround)
            {
                Debug.Log("boom");
                Explode();
                hasExploded = true;
            }
        }
        
    }
    
    private void BounceGrenade(Vector3 reflection)
    { 
        rigidBody.velocity = Vector3.Reflect(-velocityOnImpact * energyMuliplierOnBounce, reflection); 
    }
    

}
