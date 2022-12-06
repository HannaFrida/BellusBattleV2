using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    private bool respawn = false;
    private float respawnTimer;
    [SerializeField] private float respawWait = 5f;
    [SerializeField] private GameObject respawnObject;
    GameObject SpawnObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (respawn)
        {
            respawnTimer += Time.deltaTime;
            //Debug.Log("droppper: " + dropTimer + " poda " + timeToWaitForPickup);
            if (respawnTimer >= respawWait)
            {
                respawnTimer = 0f;
                respawn = false;
                SpawnObj = Instantiate(respawnObject, transform.position, transform.rotation);
            }
        }
        if (SpawnObj == null)
        {
            respawn = true;
        }
    }

    public void StartRespawn()
    {
        respawn = true;
    }
}
