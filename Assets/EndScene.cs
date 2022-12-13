using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public GameObject[] guns;
    [SerializeField] private WeaponSpawnerManager weaponSpawnerManager;
    void Update()
    {
        weaponSpawnerManager = FindObjectOfType<WeaponSpawnerManager>().GetComponent<WeaponSpawnerManager>();
        if (guns == null)
        {
            guns = GameObject.FindGameObjectsWithTag("Weapon");
            Debug.Log("found guns");
        }
            
        

        foreach (GameObject gun in guns)
        {
            if (weaponSpawnerManager.GetTrashBin != null)
            {
                gun.transform.SetParent(weaponSpawnerManager.GetTrashBin);
                Debug.Log("set in bin");
            }
        }
    }
}
