using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Martin Wallmark
*/
public class Portal : MonoBehaviour
{
    [SerializeField] private Portal teleportDestination;
    [SerializeField] private float teleportCoolDownTime;
    private float timer;
    [SerializeField]private bool canTeleport;


    public bool CanTeleport
    {
        get => canTeleport;
        set => canTeleport = value;
    }

    private void Update()
    {
        teleporterCoolDown();
    }


    private void OnTriggerEnter(Collider other)
    {

        if (canTeleport == false) return;
        if (other.gameObject.tag.Equals("Player") || other.gameObject.tag.Equals("Grenade") || other.gameObject.tag.Equals("Bullet"))
        {
            if (teleportDestination == null) return;
			SoundManager.Instance.PortalSound();
            other.gameObject.transform.position = teleportDestination.transform.position;
            teleportDestination.CanTeleport = false;
        }
    }

    private void teleporterCoolDown()
    {
        if (canTeleport) return;

        timer += Time.deltaTime;
        if(timer >= teleportCoolDownTime)
        {
            canTeleport = true;
            timer = 0f;
        }
    }



   
}
