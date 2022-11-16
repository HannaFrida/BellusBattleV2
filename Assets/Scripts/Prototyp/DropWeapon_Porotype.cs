using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon_Porotype : MonoBehaviour
{
    [SerializeField] GameObject Sword;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Sword.GetComponent<Sword_Prototype>().canAttack)
        {
            if (other.transform.tag == "Revolver")
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<Weapon>().enabled = false;

                //other.gameObject.transform.Find("revolver").gameObject.SetActive(false);
                //other.gameObject.transform.Find("Sword").gameObject.SetActive(true);
                Debug.Log("Droped Revolver");
            }

            if (other.transform.tag == "Grenade")
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<Weapon>().enabled = false;
                Debug.Log("Droped Grenade");
            }
        }
        
    }
}
