using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;



public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private bool spawnInScoreOrder;

    private int playerCount;
    private bool runTimer;
    private float timer = 3;

    public Transform[] SpawnLocations
    {
        get { return spawnLocations; }
    }

    private void Start()
    {
        
        timer = 3f;
        runTimer = true;
        playerCount = GameManager.Instance.GetAllPlayers().Count;
        if(playerCount == 0)
        {
            LookForPlayers();
        }
        else
        {
            players = GameManager.Instance.GetAllPlayers().ToArray();
        }
        SpawnPlayers();
        

    }
    private void Update()
    {
        UpdateText();
        if (runTimer == false) return;
        if(timer <= 0.5)
        {
            GameManager.Instance.ActivateMovement();  
        }
        if(timer <= 0)
        {
            runTimer = false;
            GameManager.Instance.IsRunningRoundTimer = true;
        }
        timer -= Time.deltaTime;  
    } 

    private void UpdateText()
    {
        if (countDownText == null) return;

        if (timer <= 0f)
        {
            countDownText.enabled = false;
        }
        else if (timer <= 0.5f)
        {
            countDownText.text = "Battle!";
        }
        else if(timer >= 0.5f)
        {
            countDownText.text = Mathf.RoundToInt(timer).ToString();
        }
        
    }

    private void SpawnPlayersInRandomOrder()
    {
        List<Transform> randomSpawns = new List<Transform>(spawnLocations);
        randomSpawns = randomSpawns.OrderBy(i => Random.value).ToList();
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            players[i].transform.position = randomSpawns[i].transform.position;
        }
    }

    private void SpawnPlayers()
    {
        if(spawnInScoreOrder == true)
        {
            SpawnBasedByScore();
        }
        else
        {
            //SpawnPlayersBasedOnID();
            SpawnPlayersInRandomOrder();
        }
    }

    private void SpawnPlayersBasedOnID()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            //players[i].GetComponent<Dash>().ResetValues();
            players[i].transform.position = spawnLocations[i].position;
        }
    }

    private void SpawnBasedByScore()
    {
        int[] scoreOrder = GameDataTracker.Instance.GetScoreInOrder();
        for(int i = 0; i < scoreOrder.Length; i++)
        {
            FindPlayerToSpawn(scoreOrder[i], i);
        }
        GameDataTracker.Instance.WriteToFile();
    }
    private void FindPlayerToSpawn(int id, int index)
    {
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].GetComponent<PlayerDetails>().playerID == id)
            {
                players[i].transform.position = spawnLocations[index].transform.position;
            }
        }
    }

    private void LookForPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
