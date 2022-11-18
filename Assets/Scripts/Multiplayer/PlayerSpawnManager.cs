using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] protected ScoreManager scoreManager;
    private bool hasSpawnedAllPlayers;
    private int amountOfPlayer;
    private int spawnedPlayers;

    public Transform[] SpawnLocations
    {
        get { return spawnLocations; }
    }

    private void Start()
    {

        //scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        players = GameManager.Instance.GetAllPlayers().ToArray();
        amountOfPlayer = GameManager.Instance.GetAllPlayers().Count;


    }

    private void Update()
    {
        /*
        foreach (GameObject player in GameManager.Instance.GetAllPlayers())
        {
            player.SetActive(true);
        }
        */
        // Used for when changing level
        if (spawnedPlayers == amountOfPlayer) return;
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("spawnmePls");
            players[i].gameObject.SetActive(true);
            players[i].GetComponent<Dash>().ResetValues();
            //players[i].GetComponent<PlayerHealth>().UnkillPlayer();
            players[i].transform.position = spawnLocations[i].position;
            spawnedPlayers++;
            //StartCoroutine(waitTime(10));


        }
    }

    private void SpawnPlayer()
    {
        /*
        Debug.Log("spawnmePls");
        players[spawnedPlayers].gameObject.SetActive(true);
        players[spawnedPlayers].GetComponent<Dash>().ResetValues();
        //players[i].GetComponent<PlayerHealth>().UnkillPlayer();
        players[spawnedPlayers].transform.position = spawnLocations[i].position;
        spawnedPlayers++;
        StartCoroutine(waitTime(10));
        */
    }

    private void OnLevelWasLoaded(int level)
    {
        /*
        for (int i = 0; i < players.Length; i++)
            players[i].gameObject.SetActive(true);
        */
    }

    private IEnumerator waitTime(float wait)
    {
        print("wait");
        yield return new WaitForSeconds(wait);
    }
}
