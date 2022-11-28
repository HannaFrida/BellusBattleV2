using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] protected ScoreManager scoreManager;
    private bool runTimer;
    private int amountOfPlayer;
    private int spawnedPlayers;
    private float timer;
    private float movementTurnOnDelay;

    public Transform[] SpawnLocations
    {
        get { return spawnLocations; }
    }

    private void Start()
    {
        timer = 0f;
        runTimer = true;
        amountOfPlayer = GameManager.Instance.GetAllPlayers().Count;
        movementTurnOnDelay = amountOfPlayer;
        players = GameManager.Instance.GetAllPlayers().ToArray();
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            players[i].GetComponent<Dash>().ResetValues();
            players[i].transform.position = spawnLocations[i].position;
        }
        


    }
   

    private void Update()
    {
        if (runTimer == false) return;
        Debug.Log("runnnig");
        if(timer >= movementTurnOnDelay)
        {
            GameManager.Instance.ActivateMovement();
            runTimer = false;
            timer = 0f;
        }
        timer += Time.deltaTime;
      
        
    }

  
}
