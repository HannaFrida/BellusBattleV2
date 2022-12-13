using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject anchor;
    [SerializeField] private Collider[] colliders;
    private SoundManager soundManager;
    private List<Collider> colliderList = new List<Collider>();
    private BoxCollider boxCollider;
    private GameObject currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
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
                    soundManager.OpenDoorSound();
                }
                else if(col.bounds.center.x < boxCollider.bounds.center.x)
                {
                    RotateAnchor(-90f);
                    soundManager.OpenDoorSound();
                }
                return;
            }
            else if( currentPlayer == null && anchor.transform.localRotation.y != 0f)
            {
                anchor.transform.localEulerAngles = Vector3.zero;
                soundManager.CloseDoorSound();

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
