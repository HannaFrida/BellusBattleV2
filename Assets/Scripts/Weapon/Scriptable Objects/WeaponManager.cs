using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Hanna Rudöfors
*/
public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponSlot;
    [SerializeField]
    private WeaponData equippedWeapon;
    private GameObject currentWeapon;
    [SerializeField] private GameObject rightArmAimPivotpoint;
    public WeaponData EquippedWeapon { get => equippedWeapon; }

    
    private void OnLevelWasLoaded(int level)
    {
        if (weaponSlot.childCount > 0)
        {
            currentWeapon = weaponSlot.GetChild(0).gameObject;
            UnEquipWeapon(currentWeapon);
            currentWeapon.SetActive(false);
            currentWeapon.transform.SetParent(null);
        }  
    }

    public void EquipWeapon(WeaponData weaponData, GameObject nowWeapon)
    {
        if (equippedWeapon != null)
        {
            return;
        }
        if (weaponSlot.transform.childCount == 1)
        {
            return;
        }
        equippedWeapon = weaponData;
        SoundManager.Instance.PickUpWeaponSound();

        nowWeapon.transform.SetParent(weaponSlot);
        nowWeapon.transform.localPosition = Vector3.zero;
        nowWeapon.transform.localRotation = Quaternion.identity;
        nowWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    public void UnEquipWeapon(GameObject nowWeapon)
    {
        equippedWeapon = null;
        nowWeapon = null;
    }
    
}
