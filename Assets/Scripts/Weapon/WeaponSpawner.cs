using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField]private Gun[] weapons;
    private WeaponSpawnerManager manager;

    void Start()
    {
        manager = gameObject.GetComponentInParent<WeaponSpawnerManager>();
    }
    public void SpawnRandomWeapon()
    {
        //if (isholdingWeapon) return;
        Instantiate(weapons[Random.Range(0, weapons.Length)], transform);
    }
    public void WeaponIsPicked() // Den här ska försättas med PickUp Script
    {
        manager.AddEmptySpawnerToChooseFrom(this);
    }

    public bool HasWeapons()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        foreach(Collider col in colliders)
        {
            if (col.gameObject.tag.Equals("Weapon"))
            {
                return true;
            }
        }
        return false;
        
    }
}
