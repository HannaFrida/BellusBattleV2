using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int playerID; // Stores the players ID
    public Vector3 startPos; // Stores the start spawn position
    public bool isAlive;
    [SerializeField] private Transform headGearSlot;

    public Transform HeadGearSlot()
    {
        return headGearSlot;
    }
    
   
    void Start()
    {
        transform.position = startPos; // Puts the player on the spawn position
        isAlive = true;
        //headGearSlot = GameObject.FindGameObjectWithTag("HeadSlot").transform;
    }



   
}
