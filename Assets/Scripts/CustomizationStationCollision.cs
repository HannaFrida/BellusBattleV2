using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class CustomizationStationCollision : MonoBehaviour
{
    [SerializeField]GameObject[] tempPlayerCollider = new GameObject[4];

    private void Update()
    {
       List<GameObject> players = GameManager.Instance.GetAllPlayers();
       for(int i = 0; i < players.Count; i++)
        {
            tempPlayerCollider[i].transform.position = players[i].transform.position;
        }
    }


}
