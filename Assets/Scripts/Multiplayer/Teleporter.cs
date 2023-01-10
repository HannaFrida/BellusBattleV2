using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/*
* Author Hanna Rudöfors
*/
public class Teleporter : MonoBehaviour
{
    private int playerAmountOnTeleporter = 0; // Amount of players on the Teleporter
    [SerializeField] private PlayerJoinManager playerJoinManager; // Keeps track of players in game
    [SerializeField] private LevelManager manager; // Keeps track of players in game
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject playPanel;

    private void Start()
    {
        playPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a player stands on the Teleporter the playerAmountOnTeleporter goes up
        if (other.gameObject.tag.Equals("Player"))
        {
            playerAmountOnTeleporter++;
            soundManager.PlayerOnPlay_Sound();
            soundManager.FadeInBellSounds();


        }

        // There needs to be at least two players in the scene
        // All players in game needs to be in the Teleporter for the game to start
        if (playerJoinManager.listOfPlayers.Count >= 2 && playerAmountOnTeleporter == playerJoinManager.listOfPlayers.Count && other.gameObject.GetComponent<PlayerMovement>() != null)
        { 
            playPanel.SetActive(true);
            foreach(GameObject player in GameManager.Instance.GetAllPlayers())
            {
                player.gameObject.GetComponent<NavigateUI>().SetConnection(playPanel);
                player.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
            }
            
        }
        if (playerAmountOnTeleporter == playerJoinManager.listOfPlayers.Count)
        {
            soundManager.AllPlayersOnPlay_Sound();
            soundManager.FadeOutBellSounds();
        }
        
            
        
    }

    private void OnTriggerExit(Collider other)
    {
        // When a player gets off the Teleporter the playerAmountOnTeleporter goes down
        if (other.gameObject.tag.Equals("Player"))
        {
            playerAmountOnTeleporter--;
        }
    }
    
}
