using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations; // Keeps track of all the possible spawn locations
    [SerializeField] private GameObject[] players;
    [SerializeField] private TextMeshProUGUI countDownText;

    private int playerCount;
    private bool runTimer;
    private float timer = 3;
    private float movementTurnOnDelay = 3f;

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
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            players[i].GetComponent<Dash>().ResetValues();
            players[i].transform.position = spawnLocations[i].position;
        }

    }
    private void Update()
    {
        UpdateText();
        if (runTimer == false) return;
        Debug.Log("runnnig");
        if(timer <= 0.5)
        {
            GameManager.Instance.ActivateMovement();  
        }
        if(timer <= 0)
        {
            runTimer = false;
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

    private void LookForPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
}
