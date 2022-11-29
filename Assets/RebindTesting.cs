using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindTesting : MonoBehaviour
{
    [SerializeField] private GameObject rebindMenuPanel;
    [SerializeField] GameObject playerid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    private void Update()
    {
        if (rebindMenuPanel.activeInHierarchy)
        {
           // Time.timeScale = 0;
        }
    }
}
