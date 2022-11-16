using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Prototype : MonoBehaviour
{
    [SerializeField] float timer = 3;
    float countDown;
    bool exploded;
    float radius = 3;

    public GameObject explosionParticles;

    // Update is called once per frame
    private void Start()
    {
        countDown = timer;
    }
    void FixedUpdate()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0 && !exploded)
        {
            explode();
        }
    }
    void explode()
    {
        GameObject GrenadeIns = Instantiate(explosionParticles, transform.position, transform.rotation);
        Destroy(GrenadeIns, 2);

        Collider[] collider = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider near in collider)
        {
            Rigidbody rb = near.GetComponent<Rigidbody>();
            if(rb!= null)
            {
                rb.AddExplosionForce(3000,transform.position,radius);
            }
        }
        
        exploded = true;
        Destroy(gameObject);
    }

}
