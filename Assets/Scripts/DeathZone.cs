using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private CameraFocus CF;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            //other.gameObject.GetComponent<PlayerDetails>().isAlive = false;
            //other.gameObject.GetComponent<PlayerMovement>().StopPlayer();
            CF.RemoveTarget(other.transform);
        }
    }
}
