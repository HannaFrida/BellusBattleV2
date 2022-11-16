using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private int playerAmountOnTeleporter = 0; // Amount of players on the Teleporter
    [SerializeField] private PlayerJoinManager playerJoinManager; // Keeps track of players in game
    [SerializeField] private LevelManager manager; // Keeps track of players in game
    //[SerializeField] private string startSceneName; // The name of the scene that is the beginner scene
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
        }

        // There needs to be at least two players in the scene
        // All players in game needs to be in the Teleporter for the game to start
        if (playerJoinManager.listOfPlayers.Count >= 1 && playerAmountOnTeleporter == playerJoinManager.listOfPlayers.Count)//playerSpawnManager.listOfPlayers.Count >= 2 && playerAmountOnTeleporter == playerSpawnManager.listOfPlayers.Count)
        {
            playPanel.SetActive(true);
            

            //SceneManager.LoadScene(startSceneName);
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
