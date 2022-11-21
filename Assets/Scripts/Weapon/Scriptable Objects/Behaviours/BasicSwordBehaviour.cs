using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwordBehaviour : MonoBehaviour
{
    [SerializeField] private int playerHoldingThisWeaponID;
    private PlayerMovement playerMovement;

    [SerializeField] private Transform swordPointDirection;
    [SerializeField] Gun gun;
    public bool isAttacking = false;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            Vector2 forceDir = swordPointDirection.up;

            if (other.transform.tag == "Player")
            {
                // Find the ID of the player it's colliding with
                playerHoldingThisWeaponID = gun.OwnerID;

                if (!other.gameObject.GetComponent<PlayerDetails>().playerID.Equals(playerHoldingThisWeaponID))
                {
                    playerMovement = other.gameObject.GetComponent<PlayerMovement>();
                    playerMovement.AddExternalForce(forceDir * 50f);
                    Debug.Log("Knocked back player");
                }
                isAttacking = false;

            }
        }
        
    }
}
