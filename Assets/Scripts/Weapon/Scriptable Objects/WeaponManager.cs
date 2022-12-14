using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    public Transform weaponSlot;

    [SerializeField]
    private WeaponData equippedWeapon;

    private GameObject currentWeapon;
    //[SerializeField] private GameObject rightArmAimPivotpoint;

    public WeaponData EquippedWeapon { get => equippedWeapon; }


    private void OnLevelWasLoaded(int level)
    {
        OnlyOneChild();

    }

    public void OnlyOneChild()
    {
        if (weaponSlot.childCount > 0)
        {
            currentWeapon = weaponSlot.GetChild(0).gameObject;
            UnEquipWeapon(currentWeapon);
            currentWeapon.GetComponent<Gun>().Drop();
            //currentWeapon.SetActive(false);
            //currentWeapon.transform.SetParent(null);
        }
    }

    private void Update()
    {
        /*
        if (equippedWeapon.name == "RailGun")
        {
            weaponSlot.transform.rotation.y = -80f;
        }
        else{
            weaponSlot.transform.rotation.y = -99.742f;
        }
        */

    }

    public void EquipWeapon(WeaponData weaponData, GameObject nowWeapon)
    {
        if (equippedWeapon != null)
        {
            return;
        }
        equippedWeapon = weaponData;

        if (equippedWeapon.pickupSound != null)
        {
            equippedWeapon.pickupSound.Play();
        }

        nowWeapon.transform.SetParent(weaponSlot);
        nowWeapon.transform.localPosition = Vector3.zero;
        nowWeapon.transform.localRotation = Quaternion.identity;
        nowWeapon.GetComponent<BoxCollider>().enabled = false;
    }

    public void UnEquipWeapon(GameObject nowWeapon)
    {

        //nowWeapon.GetComponent<Gun>().Drop();
        
        equippedWeapon = null;
        nowWeapon = null;
    }

}
