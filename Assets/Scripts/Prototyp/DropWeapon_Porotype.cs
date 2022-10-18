using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon_Porotype : MonoBehaviour
{
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
        if(other.transform.tag == "Player")
        {
            other.GetComponent<Shoot>().enabled = false;
            gameObject.GetComponent<Sword_Prototype>().enabled = true;

        }
    }
}