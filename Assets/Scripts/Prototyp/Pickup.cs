using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject pickUp;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Shoot>().enabled = true;
            other.GetComponent<Shoot>().eqiup(pickUp);
            
            
        }
    }
}
