using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Sword_Prototype : MonoBehaviour
{

    public GameObject Sword;
    Vector3 pos;
    Vector3 rotation;
    public bool canAttack = true;
    public float cooldown = 3;
    public Transform ShootPoint;
    public UnityEvent shootEvent;

    Collider[] hitColliders;
    private void Start()
    {
        
    }


    public void Attack(InputAction.CallbackContext ctx)
    {
        hitColliders = Physics.OverlapSphere(transform.position, 2f);
        pos = Sword.transform.position;
        if (!ctx.performed) { return; }
        if (canAttack)
        {
            Sword.transform.position += ShootPoint.transform.up * 1.02f;
            shootEvent.Invoke();
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("Revolver"))
                {
                    //Debug.Log(col.gameObject.name);
                    col.gameObject.SetActive(false);

                    col.GetComponent<MeshRenderer>().enabled = true;
                    col.GetComponent<Weapon>().enabled = true;
                    Debug.Log("disarm revolver");

                }
                if (col.CompareTag("Grenade"))
                {
                    //Debug.Log(col.gameObject.name);
                    col.gameObject.SetActive(false);

                    col.GetComponent<MeshRenderer>().enabled = true;
                    col.GetComponent<Weapon>().enabled = true;
                    Debug.Log("disarm grenade");
                }

                    //gameObject.GetComponentInChildren<DropWeapon_Porotype>().enabled = true;
                    //gameObject.GetComponentInChildren<BoxCollider>().enabled = true;

            }
                    canAttack = false;
            StartCoroutine(ResetAttack());
            //Debug.Log(pos);
        }

        
    }

    IEnumerator ResetAttack()
    {
        //Debug.Log(pos);
        Sword.transform.position = pos;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
        //gameObject.GetComponentInChildren<DropWeapon_Porotype>().enabled = false;
        //gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }

}
