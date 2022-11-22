using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private List<Collider> attachedColliders = new List<Collider>();
    [SerializeField] private BoxCollider attachZone;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField]  Transform targetOne;
    [SerializeField] Transform targetTwo;
    private Collider[] collidersOnPlatform;
    private Transform currentTarget;
    [SerializeField] private Vector2 movementDirection;

    private void OnLevelWasLoaded(int level)
    {
        attachedColliders.Clear();
        collidersOnPlatform = null;
    }

    private void Start()
    {
        CheckTargetsVerticalPosition();
        if (targetOne == null) return;
        
        currentTarget = targetOne;
        
        
        CreateDirectionVector();
    }

    private void Update()
    {
        if (targetOne == null || targetTwo == null) return;
        MovePlatform();
        AttachPlayers();
        DetachPlayer();
        MovePlayers();
    }

    private void AttachPlayers()
    {
        collidersOnPlatform = Physics.OverlapBox(attachZone.bounds.center, attachZone.size / 2);

        foreach(Collider col in collidersOnPlatform)
        {
            if (col.gameObject.CompareTag("Player") && attachedColliders.Contains(col) == false)
            {
                attachedColliders.Add(col);
                //col.gameObject.transform.parent = transform;
            }
        }
    }

    private void DetachPlayer()
    {
        if (attachedColliders.Count == 0) return;

        List<Collider> currentColliders = new List<Collider>(collidersOnPlatform);
        for(int i = attachedColliders.Count - 1; i >= 0; i--)
        {
            if(currentColliders.Contains(attachedColliders[i]) == false)
            {
                attachedColliders[i].GetComponent<PlayerMovement>().IsMovedByPLatform = false;
                //attachedColliders[i].gameObject.transform.parent = null;
                attachedColliders.RemoveAt(i);
                
            }
            
        }
    }

    private void MovePlayers()
    {
        foreach(Collider col in attachedColliders)
        {
            Vector2 addedForce;
            if(currentTarget == targetOne)
            {
                addedForce = -movementDirection;
            }
            else
            {
                addedForce = movementDirection;
            }
            PlayerMovement playerMovement = col.GetComponent<PlayerMovement>();
            playerMovement.AddConstantExternalForce(addedForce * moveSpeed);
            playerMovement.IsMovedByPLatform = true;
        }
    }

    private void MovePlatform()
    {
        if(transform.position != currentTarget.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            currentTarget = currentTarget == targetOne ? currentTarget = targetTwo : targetOne;
        }
        

        if (attachedColliders.Count == 0) return;
        MovePlayers();
    }

    private void CreateDirectionVector()
    {
        movementDirection = (transform.position - currentTarget.position).normalized;
    }

    private void CheckTargetsVerticalPosition()
    {
        if (targetOne == null || targetTwo == null) return;
        targetOne.position = new Vector2(targetOne.position.x, transform.position.y);
        targetTwo.position = new Vector2(targetTwo.position.x, transform.position.y);
    }
}
