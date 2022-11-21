using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp_ProtoV1 : MonoBehaviour
{

    Collider[] hitColliders;
    [SerializeField] private Transform weaponPosition;
    private Weapon currentWeapon;
    [SerializeField] GameObject revolver;
    [SerializeField] GameObject Grenade;
    [SerializeField] GameObject Sword;
    public bool isHoldingWeapon;

    private void PickUpWeapon(Vector3 center, float radius)
    {
        hitColliders = Physics.OverlapSphere(center, radius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Revolver") && !isHoldingWeapon)
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);

                revolver.GetComponent<MeshRenderer>().enabled = true;
                revolver.GetComponent<Weapon>().enabled = true;
                revolver.GetComponent<Weapon>().ammo = 6;

                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                Sword.GetComponentInChildren<MeshRenderer>().enabled = false;
                Sword.GetComponent<Sword_Prototype>().enabled = false;
                Sword.GetComponentInChildren<DropWeapon_Porotype>().enabled = false;

                isHoldingWeapon = true;

                return;
            }
            if (col.CompareTag("Grenade") && !isHoldingWeapon)
            {
                Debug.Log(col.gameObject.name);
                col.gameObject.SetActive(false);
                //Debug.Log("1");

                Grenade.GetComponent<MeshRenderer>().enabled = true;
                Grenade.GetComponent<Weapon>().enabled = true;
                revolver.GetComponent<Weapon>().ammo = 1;
                //Debug.Log("2");
                //currentWeapon = col.gameObject.GetComponent<Weapon>();
                //currentWeapon.transform.parent = weaponPosition.transform;
                Sword.GetComponentInChildren<MeshRenderer>().enabled = false;
                Sword.GetComponent<Sword_Prototype>().enabled = false;
                Sword.GetComponentInChildren<DropWeapon_Porotype>().enabled = false;
                //Debug.Log("3");

                isHoldingWeapon = true;
                
                return;
            }
        }
    }

    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PickUpWeapon(transform.position, 2f);
        }
    }
}
