using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuPanel;

    private void OnTriggerEnter(Collider other)
    {
        // When a player stands on the Teleporter the playerAmountOnTeleporter goes up
        if (other.gameObject.tag == "Player")
        {
            settingsMenuPanel.SetActive(true);
        }
    }

    
}
