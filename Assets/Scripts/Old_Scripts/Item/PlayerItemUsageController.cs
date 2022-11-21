using ItemNamespace;
using UnityEngine;
using System;
using Inventory_scripts;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerItemUsageController : MonoBehaviour
{
    /*
    public ItemBase itemBase; // Will need to be updated if another item is being used.

    [SerializeField] public GameObject heldItemWorldObject;

    private Type currentActingComponentType;
    private ItemBaseBehaviour currentActingComponent;
    private bool whenUsingItem;


    public void Start()
    {
        ChangeItem(itemBase);
    }

    public void OnUse(InputAction.CallbackContext ctx)
    {
        //Checks if button is pressed and if there is an item in the player hand triggers that items use function
        if (ctx.performed)
        {
            if (itemBase != null)
            {
                currentActingComponent.Use(itemBase);
            }
        }
    }

    //Changes what item the player has in their hand and sets the correct mesh and material
    public void ChangeItem(ItemBase newItemBase)
    {
        itemBase = newItemBase; // updates itembase
        Type itemType = Type.GetType(itemBase.GetItemBaseBehaviorScriptName); // fetches type of the itembehaviour


        if (currentActingComponent != null)
        {
            currentActingComponent.StopAnimation();
            Destroy(currentActingComponent);
        }

        currentActingComponent = (ItemBaseBehaviour)gameObject.AddComponent(itemType);
        currentActingComponent.SetBelongingTo(itemBase);
        currentActingComponentType = itemType;
        heldItemWorldObject.GetComponent<MeshFilter>().mesh = itemBase.GetMesh;
        heldItemWorldObject.GetComponent<MeshRenderer>().material = itemBase.GetMaterial;

        // swordPrefab = GameObject.FindGameObjectWithTag("Sword");
        //Instantiate(swordPrefab, heldItemWorldObject.transform);
    }
    */
}