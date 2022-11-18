using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] protected ScoreManager scoreManager;

    public Transform[] SpawnLocations
    {
        get { return spawnLocations; }
    }

    private void Start()
    {
        
        //scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        foreach (GameObject player in GameManager.Instance.GetAllPlayers())
        {
            player.SetActive(true);
        }
        players = GameManager.Instance.GetAllPlayers().ToArray(); // Used for when changing level
        
        for(int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].GetComponent<Dash>().ResetValues();
            //players[i].GetComponent<PlayerHealth>().UnkillPlayer();
            players[i].transform.position = spawnLocations[i].position;
            
        }
        
    }

    private void OnLevelWasLoaded(int level)
    {
        /*
        for (int i = 0; i < players.Length; i++)
            players[i].gameObject.SetActive(true);
        */
    }
}
