using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponSlot;

    [SerializeField]
    private WeaponData equippedWeapon;

    private GameObject currentWeapon;
    bool hasflippedRight;
    bool hasflippedLeft;
    [SerializeField] private GameObject rightArmAimPivotpoint;

    public WeaponData EquippedWeapon { get => equippedWeapon; }

    private void Start()
    {
        
    }

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

    private void Update()
    {
        /*
        Debug.Log(rightArmAimPivotpoint.transform.eulerAngles.z);

        if(rightArmAimPivotpoint.transform.eulerAngles.z > 90f && rightArmAimPivotpoint.transform.eulerAngles.z < 275f && hasflippedLeft == false)
        {
            //Debug.Log(rightArmAimPivotpoint.transform.rotation.z);
            hasflippedLeft = true;
            hasflippedRight = false;
            weaponSlot.eulerAngles = new Vector3(0f, weaponSlot.rotation.y, weaponSlot.rotation.z);
        }
        else if(rightArmAimPivotpoint.transform.eulerAngles.z < 90f && rightArmAimPivotpoint.transform.eulerAngles.z > 275f &&  hasflippedRight == false)
        {
            hasflippedRight = true;
            hasflippedLeft = false;
            weaponSlot.eulerAngles = new Vector3(180f, weaponSlot.rotation.y, 180f);
        }
        */

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
