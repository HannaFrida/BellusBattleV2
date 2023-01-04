using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


public class GameDataTracker : MonoBehaviour
{

    public static GameDataTracker Instance;
    private Dictionary<int, float> roundTimeDic = new Dictionary<int, float>();
    private Dictionary<int, int> roundWinnerDic = new Dictionary<int, int>();
    private Dictionary<int, List<KillEvent>> killsEachRoundDic = new Dictionary<int, List<KillEvent>>();
    private Dictionary<int, int> playerScore = new Dictionary<int, int>();
    private Dictionary<int, int> playerKills = new Dictionary<int, int>();
    private List<KillEvent> killList = new List<KillEvent>();
    private int playersKilledByHazard;
    private int totalRoundsPlayed;
    private float totalGameTime;
    private string filePath; 
    private bool isInEditor;

    private int currentRound;
    // Start is called before the first frame update

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
    }
    private void Start()
    {
        if (Application.isEditor)
        {
            isInEditor = true;
        }
        filePath = GetFilePath();
    }

    private string GetFilePath()
    {
        if (isInEditor)
        {
            return "Assets/Resources/GameLogs.txt";
        }
        else
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/GameLogs/");
            return Application.streamingAssetsPath + "/GameLogs/Logs.txt";

        }
        
    }

    public void SetCurrentRound(int roundNr)
    {
        currentRound = roundNr;
    }
   
    public void NewKillEvent(int killer, int killed, string weaponName, float timeOfKill)
    {
        if(killsEachRoundDic.ContainsKey(currentRound)== false)
        {
            killsEachRoundDic.Add(currentRound, new List<KillEvent>());
        }
        KillEvent killEvent = new KillEvent(killerID: killer, killedPlayerID: killed, weaponName, timeOfKill);
        killList.Add(killEvent);
        if(killer != killed)
        {
            killsEachRoundDic[currentRound].Add(killEvent);
            AddNewKill(killer);
        }
        

    }

    public void AddWinner(int roundNr, int playerID)
    {

        if(roundWinnerDic.ContainsKey(roundNr) == false)
        {
            roundWinnerDic.Add(roundNr, playerID);
        }
        else
        {
            roundWinnerDic[roundNr] = playerID;
        }

        if(playerScore.ContainsKey(playerID) == false)
        {
            playerScore.Add(playerID, 1);
        }
        else
        {
            playerScore[playerID]++;
        }
        
    }

    public void AddNewKill(int id)
    {
        if(playerKills.ContainsKey(id) == false)
        {
            playerKills.Add(id, 1);
        }
        else
        {
            playerKills[id]++;
        }
    }

    public string StreakFinder()
    {
        int roundWinner = -1; // ingenting har -1 som id
        int streak = 0;
        for(int i = currentRound; i > 0; i--)
        {
            if (roundWinner == -1)
            {
                roundWinner = roundWinnerDic[i];
            }
            if(roundWinner == roundWinnerDic[i])
            {
                streak++;
            }
            else
            {
                break;
            }
            
        }
        if(streak > 1)
        {
            if(roundWinner != 0)
            {
                return $"P{roundWinner} won {streak} rounds in a row";
            }
            return $"{streak} rounds have ended in a draw";
            
        }
        return "nothing interesting";
    }

    public string MultiKillFinder()
    {
        Dictionary<int, List<float>> killerAndTime = new Dictionary<int, List<float>>();
        KillStreak killStreak;
        string returnValue = "nothing interesting";
        if (killsEachRoundDic.ContainsKey(currentRound) == false) return returnValue;

        for (int i = 0; i < killsEachRoundDic[currentRound].Count; i++)
        {
            if (killsEachRoundDic[currentRound][i].GetKiller() == 0) continue;
            if(killerAndTime.ContainsKey(killsEachRoundDic[currentRound][i].GetKiller()) == false)
            {
                killerAndTime[killsEachRoundDic[currentRound][i].GetKiller()] = new List<float>();
            }
            killerAndTime[killsEachRoundDic[currentRound][i].GetKiller()].Add(killsEachRoundDic[currentRound][i].GetKillTime());
        }
        for(int i = 1; i <=4; i++)
        {
            if (killerAndTime.ContainsKey(i) == false) continue;

            if (killerAndTime[i].Count >= 2)
            {
                int amountOfKills = killerAndTime[i].Count;
                float timeDiff = killerAndTime[i][amountOfKills - 2] - killerAndTime[i][0];
                killStreak = new KillStreak(i, amountOfKills, timeDiff);
                returnValue = killStreak.ToString();
                break;
            }
        }
        return returnValue;
    }

    public int[] GetScoreInOrder()
    {
        int index = 0;
        int previousPlayerID = 0;
        ValidatePlayerScore();
        List<KeyValuePair<int, int>> scoreOrder = playerScore.ToList();
        scoreOrder.Sort((score1, score2) => score1.Value.CompareTo(score2.Value));
        List<int> order = new List<int>();
        foreach(KeyValuePair<int,int> playerId in scoreOrder)
        {
            Debug.Log(previousPlayerID);
            if (playerId.Key == 0) continue;
            if(playerScore.ContainsKey(previousPlayerID) && playerId.Value == playerScore[previousPlayerID])
            {
                Debug.Log(previousPlayerID);
                SwapListPosition(order, playerId.Key, previousPlayerID, index);
            }
            else
            {
                order.Add(playerId.Key);
            }

            previousPlayerID = order.Last();
            index++;
        }
        order.Reverse();
        return order.ToArray();
    }

    private void SwapListPosition(List<int> list, int currentPlayer, int previousPlayer, int previousIndex)
    {
        if(list.Count != 1 && playerKills.ContainsKey(currentPlayer) && playerKills.ContainsKey(previousPlayer) == false || playerKills[currentPlayer] <= playerKills[previousPlayer])
        {
            list[previousIndex] = currentPlayer;
            list.Add(previousPlayer); 
        }
        else
        {
            list.Add(currentPlayer);
        }
    }

    private void ValidatePlayerScore()
    {
        foreach(GameObject player in GameManager.Instance.GetAllPlayers())
        {
            int currentID = player.GetComponent<PlayerDetails>().playerID;
            if (playerScore.ContainsKey(currentID) == false)
            {
                playerScore.Add(currentID, 0);
            }
        }
    }

    public int GetPlayerScore(int id)
    {
        if (playerScore.ContainsKey(id) == false)
        {
            return 0;
        }
        return playerScore[id];
    }

    public int GetPlayerKills(int id)
    {
        if(playerKills.ContainsKey(id) == false)
        {
            return 0;
        }
        return playerKills[id];
    }
    
    private struct KillStreak
    {
        private readonly int killer;
        private readonly int kills;
        private readonly float streakTimeSpan;

        public KillStreak(int killer, int kills, float streakTimeSpan)
        {
            this.killer = killer;
            this.kills = kills;
            this.streakTimeSpan = streakTimeSpan;
        }

        public override string ToString()
        {
            return $"P{killer} just killed {kills} players!";
        }
    }

   
    public void IncreaseRoundsPlayed()
    {
        totalRoundsPlayed++;
    }

    public void IncreasePlayersKilledByHazards()
    {
        playersKilledByHazard++;
    }

    public void WriteToFile()
    {
        StreamWriter writer = new StreamWriter(filePath, true);
        writer.WriteLine($"Session completed at {System.DateTime.Now} ---------------------------------------------");
        foreach (KillEvent eve in killList)
        {
            writer.WriteLine(eve.ToString());
        }
        writer.WriteLine();
        for (int i = 1; i <= totalRoundsPlayed; i++)
        {
            if (roundTimeDic.ContainsKey(i))
            {
                writer.WriteLine($"Round {i} lasted {roundTimeDic[i]} seconds");
                totalGameTime += roundTimeDic[i];
            }

        }
        writer.WriteLine($"Rounds played: {totalRoundsPlayed} \nTotal time of session: {totalGameTime} seconds \nAverage time per round: {totalGameTime / totalRoundsPlayed} seconds");
        writer.WriteLine($"\ntotal amount of players killed by hazards : {playersKilledByHazard} \n" +
            $"");
        writer.Close();
        ClearSavedData();

    }

    public void SaveRoundTime(int roundNr, float duration)
    {
        roundTimeDic[roundNr] = duration;
        totalRoundsPlayed++;
    }

    public void ClearSavedData()
    {
        killList.Clear();
        roundTimeDic.Clear();
        roundWinnerDic.Clear();
        killsEachRoundDic.Clear();
        playerScore.Clear();
        playerKills.Clear();
        playersKilledByHazard = 0;
        totalRoundsPlayed = 0;
        totalGameTime = 0;
    }
}

public struct KillEvent
{
    private readonly int killerID;
    private readonly int killedPlayerID;
    private readonly string weaponName;
    private readonly float timeOfKill;

    public KillEvent(int killerID, int killedPlayerID, string weaponName, float timeOfKill)
    {
        this.killerID = killerID;
        this.killedPlayerID = killedPlayerID;
        this.timeOfKill = timeOfKill;
        if (weaponName == null)
        {
            weaponName = "Unnamed hazard";
        }
        this.weaponName = weaponName;
    }

    public int GetKiller()
    {
        return killerID;
    }

    public float GetKillTime()
    {
        return timeOfKill;
    }


    public override string ToString()
    {
        if(killerID == 0)
        {
            return $"Player {killedPlayerID} got killed by {weaponName}";
        }
        else
        {
            return $"Player {killerID} killed Player {killedPlayerID} with the {weaponName}";
        }
        
    }

}
