using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    [SerializeField] private List<Collider> attachedColliders = new List<Collider>();
    [SerializeField] private BoxCollider attachZone;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField]  Transform targetOne;
    [SerializeField] Transform targetTwo;
    private Collider[] collidersOnPlatform;
    private Transform currentTarget;

    private void OnLevelWasLoaded(int level)
    {
        attachedColliders.Clear();
        collidersOnPlatform = null;
    }

    private void Start()
    {
        currentTarget = targetOne;
    }

    private void Update()
    {
        MovePlatform();
        AttachPlayers();
        DetachPlayer();
        MovePlayers();
    }

    private void AttachPlayers()
    {
        collidersOnPlatform = Physics.OverlapBox(attachZone.bounds.center, attachZone.transform.localScale / 2);

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
                //attachedColliders[i].gameObject.transform.parent = null;
                attachedColliders.RemoveAt(i);
                
            }
            
        }
    }

    private void MovePlayers()
    {
        foreach(Collider col in attachedColliders)
        {
            col.GetComponent<PlayerMovement>().AddExternalForce(new Vector2(moveSpeed, 0f));
        }
    }

    private void MovePlatform()
    {
        if(transform.position != currentTarget.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed);
        }
        else
        {
            currentTarget = currentTarget == targetOne ? currentTarget = targetTwo : targetOne;
        }
        

        if (attachedColliders.Count == 0) return;
        MovePlayers();
    }
}
