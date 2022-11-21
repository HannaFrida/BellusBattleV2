using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ItemNamespace;
using TMPro;


namespace Inventory_scripts
{
    public class PlayerInventory : MonoBehaviour
    {
        /*
        
        [SerializeField] public ItemBase[] inventory;
        //[SerializeField] private TMP_Text armorDisplayText;
        //[SerializeField] private GameObject[] sprites;
        //[SerializeField] private GameObject selectedItem;
        //[SerializeField] private GameObject meatStackNumber;
        [SerializeField] public Animator animator;
        //[SerializeField] private uint netID;
        //[SerializeField] private GameObject itemInfoSprite;
        [SerializeField] private GameObject[] weaponStats;
        [SerializeField] private PlayerItemUsageController playerItemUsageController;
        [SerializeField] private ItemBase wieldedItemBase;
        //[SerializeField] private GameObject tutorialHandlerGameObject;

        //private Guid itemPickupGuid;


        private void Start()
        {

        }
        Collider[] hitColliders;
        [SerializeField] private Transform weaponPosition;
        private Weapon currentWeapon;

        public void OnPickUp(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                OnItemPickup(transform.position, 2f);
            }
        }

        // Inserts the itembase + its sprite to the inventory array
        void OnItemPickup(Vector3 center, float radius)
        {
            hitColliders = Physics.OverlapSphere(center, radius);
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("Revolver"))
                {
                    Debug.Log(col.gameObject.name);
                    currentWeapon = col.gameObject.GetComponent<Weapon>();
                    currentWeapon.transform.parent = weaponPosition.transform;
                    return;
                }
            }



            //int weaponIndex = 2;
            // Updates the held item locally
            //UpdateHeldItem(weaponIndex);
        }

        // Activate sword when disarmed
        public void ReturnToDefault()
        {
            // Sword should be first i list
            gameObject.GetComponent<PlayerItemUsageController>().ChangeItem(inventory[1]);
        }

        
        public void UpdateHeldItem(int index)
        {
            gameObject.GetComponent<PlayerItemUsageController>().ChangeItem(inventory[index]);
        }
        */
    }

}