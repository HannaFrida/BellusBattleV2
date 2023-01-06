using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Khaled Alraas
*/
public class PTP : MonoBehaviour
{
    [SerializeField] private Transform pos;
    [SerializeField] private bool left;
    private void OnTriggerEnter(Collider other)
    {
        // When a player stands on the Teleporter the playerAmountOnTeleporter goes up
        if (other.gameObject.tag == "Player" && !left)
        {
            other.gameObject.transform.position = new Vector3(pos.position.x + 2, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            //other.gameObject.GetComponent<NavigateUI>().SetConnection(settingsMenuPanel);
        }
        else if (other.gameObject.tag == "Player" && left)
        {
            other.gameObject.transform.position = new Vector3(pos.position.x - 2, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            //other.gameObject.GetComponent<NavigateUI>().SetConnection(settingsMenuPanel);
        }
    }
}
