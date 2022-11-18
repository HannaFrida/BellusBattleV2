using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] protected ScoreManager scoreManager;
    private bool runSpawner;
    private int amountOfPlayer;
    private int spawnedPlayers;
    private float timer;
    private float timeBetweenSpawn = 1f;

    public Transform[] SpawnLocations
    {
        get { return spawnLocations; }
    }


    private void OnLevelWasLoaded(int level)
    {
        runSpawner = true;
        timer = 0f;
    }

    private void Start()
    {

        //scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        players = GameManager.Instance.GetAllPlayers().ToArray();
        amountOfPlayer = GameManager.Instance.GetAllPlayers().Count;


    }
   

    private void Update()
    {
        
        if (runSpawner == false) return;
        Debug.Log("runnnig");


        if (spawnedPlayers == amountOfPlayer) 
        {
            runSpawner = false;
        }


        if(timer >= timeBetweenSpawn)
        {
            SpawnPlayer();
            timer = 0f;
        }
        timer += Time.unscaledDeltaTime;
      
        
    }

    private void SpawnPlayer()
    {
        
        Debug.Log("spawnmePls");
        players[spawnedPlayers].gameObject.SetActive(true);
        players[spawnedPlayers].GetComponent<Dash>().ResetValues();
        players[spawnedPlayers].GetComponent<PlayerHealth>().SetPlayerVisable();
        //players[i].GetComponent<PlayerHealth>().UnkillPlayer();
        players[spawnedPlayers].transform.position = spawnLocations[spawnedPlayers].position;
        spawnedPlayers++;
        
    }

  

    private IEnumerator waitTime(float wait)
    {
        print("wait");
        yield return new WaitForSeconds(wait);
    }
}
