using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /*
    [SerializeField] private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] private CameraFocus cameraFocus;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int pointsToWin;
    [SerializeField] public List<GameObject> players = new List<GameObject>();
    [SerializeField] private bool hasGivenScore;
    [SerializeField] private float giveScoreTimer;
    [SerializeField] private bool hasOnePlayerLeft;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;

    [SerializeField] private bool gameHasStarted; //för att den inte ska börja räkna poäng i lobbyn, är tänkt att sättas till true när man går igenom teleportern
    private int winner;

    public int Winner
    {
        get { return winner; }
    }

    public bool GameHasStarted
    {
        get { return gameHasStarted; }
        set { gameHasStarted = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }



   

    private void OnLevelWasLoaded(int level)
    {
        giveScoreTimer = 0;
        if (level != 0)
        {
            gameHasStarted = true;
        }
        if (cameraFocus == null)
        {
           cameraFocus =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
        }
        if(levelManager == null)
        {
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        }
          
    }
    private void Update()
    {
        if (!gameHasStarted) return;
        CheckPlayersLeft();
      
        if(hasOnePlayerLeft && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }
       
    }

    public void AddPlayers(GameObject player)
    {
        players.Add(player);
    }
    private void ClearScore()
    {
        scoreDic.Clear();
    }

    private void CheckPlayersLeft()
    {
        if (cameraFocus._targets.Count <= 1)
        {
            hasOnePlayerLeft = true;
        }
        else if (cameraFocus._targets.Count > 1)
        {
            hasOnePlayerLeft = false;
        }
    }

    private void AddScore(GameObject winner) //TODO använd playerID istället för hela spelarobjektet
    {
        if (!scoreDic.ContainsKey(winner))
        {
            scoreDic[winner] = 1;
        }
        else
        {
            scoreDic[winner]++;
        }

    }

    public int getScore(GameObject player)
    {
        return !scoreDic.ContainsKey(player) ? 0 : scoreDic[player];
    }

    public void SetPointsToWin(int value)
    {
        pointsToWin = value;
    }

    private void GiveScoreAfterTimer()
    {
        giveScoreTimer += Time.deltaTime;
        if (giveScoreTimer <= giveScoreTime) return;

        
        if (cameraFocus._targets.Count != 0)
        {
            AddScore(cameraFocus._targets[0].transform.gameObject);
            hasGivenScore = true;
            if (getScore(cameraFocus._targets[0].transform.gameObject) == pointsToWin)
            {
                winner = cameraFocus._targets[0].transform.gameObject.GetComponent<PlayerDetails>().playerID;
                ClearScore();
                levelManager.Finish(gameObject);
                //Nån har vunnit!
                return;
            }
        }
        else
        {
            Debug.Log("Its a draaaaw!");
        }
        hasGivenScore = false;
        levelManager.LoadNextScene();





    }
    */


}
