using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter++;
            soundManager.PlayerOnPlay_Sound();
        }

        // There needs to be at least two players in the scene
        // All players in game needs to be in the Teleporter for the game to start
        if (playerJoinManager.listOfPlayers.Count >= 1 && playerAmountOnTeleporter == playerJoinManager.listOfPlayers.Count && other.gameObject.GetComponent<PlayerMovement>() != null)//playerSpawnManager.listOfPlayers.Count >= 2 && playerAmountOnTeleporter == playerSpawnManager.listOfPlayers.Count)
        {
            playPanel.SetActive(true);
            other.gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
            other.gameObject.GetComponent<NavigateUI>().SetConnection(playPanel);
        }
        if (playerAmountOnTeleporter == playerJoinManager.listOfPlayers.Count)
        {
            soundManager.AllPlayersOnPlay_Sound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When a player gets off the Teleporter the playerAmountOnTeleporter goes down
        if (other.gameObject.tag == "Player")
        {
            playerAmountOnTeleporter--;
        }
    }
    
}
