using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
* Author Khaled Alraas
*/
public class OpenSettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuPanel;

    private void OnTriggerEnter(Collider other)
    {
        // When a player stands on the Teleporter the playerAmountOnTeleporter goes up
        if (other.gameObject.tag == "Player")
        {
            settingsMenuPanel.SetActive(true);
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
                obj[i].GetComponent<NavigateUI>().SetConnection(settingsMenuPanel);
            }
            
        }
    }

    
}
