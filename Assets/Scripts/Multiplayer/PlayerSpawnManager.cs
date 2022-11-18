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


    private void OnLevelWasLoaded(int level)
    {
        
    }

    private void Start()
    {
        runTimer = true;
        timer = 0f;
        amountOfPlayer = GameManager.Instance.GetAllPlayers().Count;
        movementTurnOnDelay = amountOfPlayer;
        //scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        players = GameManager.Instance.GetAllPlayers().ToArray();
        for(int i = 0; i < players.Length; i++)
        {
            Debug.Log("spawnmePls");
            //players[i].gameObject.SetActive(true);
            players[i].GetComponent<Dash>().ResetValues();
            players[i].transform.position = spawnLocations[i].position;
            //spawnedPlayers++;
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
        timer += Time.unscaledDeltaTime;
      
        
    }

  
}
