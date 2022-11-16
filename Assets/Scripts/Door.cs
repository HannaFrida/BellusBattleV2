using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject anchor;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private int doorHealth;
    private List<Collider> colliderList = new List<Collider>();
    private BoxCollider boxCollider;
    private GameObject currentPlayer;


    public int DoorHealth
    {
        get => doorHealth;
        set => doorHealth = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void FixedUpdate()
    {
        CheckForPlayers();
        if (currentPlayer == null) return;

        colliderList = new List<Collider>(colliders);
        if (!colliderList.Contains(currentPlayer.GetComponent<Collider>()))
        {
            currentPlayer = null;
        }
      
    }

   private void CheckForPlayers()
    {
        colliders = Physics.OverlapBox(transform.position, boxCollider.size / 2, Quaternion.identity);
        foreach(Collider col in colliders)
        {
            if (col.CompareTag("Player") && col.gameObject != currentPlayer && currentPlayer == null)
            {
                currentPlayer = col.gameObject;
                if(col.bounds.center.x > boxCollider.bounds.center.x)
                {
                    RotateAnchor(90f);
                }
                else if(col.bounds.center.x < boxCollider.bounds.center.x)
                {
                    RotateAnchor(-90f);
                }
                return;
            }
            else if( currentPlayer == null)
            {
                anchor.transform.localEulerAngles = Vector3.zero;
            }
        }
        
    }

    private void RotateAnchor(float rotationAmount)
    {
        if ((anchor.transform.localEulerAngles.y == 90f && rotationAmount > 0f) || (anchor.transform.localEulerAngles.y == -90f && rotationAmount < 0f)) return;
        anchor.transform.Rotate(new Vector3(0f,rotationAmount, 0f));
    }

    public void DestroyDoor()
    {
        gameObject.SetActive(false);
    }
}
