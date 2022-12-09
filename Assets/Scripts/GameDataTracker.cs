using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


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
    // Update is called once per frame
    void Update()
    {
      
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
            killsEachRoundDic[currentRound] = new List<KillEvent>();
        }
        KillEvent killEvent = new KillEvent(killerID: killer, killedPlayerID: killed, weaponName, timeOfKill);
        killsEachRoundDic[currentRound].Add(killEvent);
        killList.Add(killEvent);
        if(killer != killed)
        {
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
        int roundWinner = 0;
        int streak = 0;
        for(int i = currentRound; i > 0; i--)
        {
            if (roundWinner == 0)
            {
                roundWinner = roundWinnerDic[i];
                Debug.Log("roundwinner is " + roundWinner);
            }
            if(roundWinner == roundWinnerDic[i])
            {
                Debug.Log("streak is " + streak);
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
                return $"Player {roundWinner} has won {streak} rounds in a row!";
            }
            return $"The last {streak} rounds have ended in a draw!";
            
        }
        return "nothing interesting";
    }

    public string MultiKillFinder()
    {
        Dictionary<int, List<float>> killerAndTime = new Dictionary<int, List<float>>();
        KillStreak killStreak;
        string returnValue = "Nothing interesting";
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
        int amountOfPlayers = GameManager.Instance.GetAllPlayers().Count;
        int[] playerOrder = new int[amountOfPlayers];
        for(int i = 1; i < amountOfPlayers; i++)
        {
            if (playerScore.ContainsKey(i) == false) continue;
            int order = 0;
            for(int j = 0; j < amountOfPlayers; j++)
            {
                if (playerScore.ContainsKey(j) == false) continue;
                if (playerScore[i] < playerScore[j])
                {
                    order++;
                }
                else
                {
                    playerOrder[order] = i;
                }
            }
        }
        return playerOrder;  
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
            return $"The Player {killer} killed {kills} players in the last round!";
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

    private void ClearSavedData()
    {
        killList.Clear();
        roundTimeDic.Clear();
        //roundWinnerDic.Clear();
        killsEachRoundDic.Clear();
        playerScore.Clear();
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
