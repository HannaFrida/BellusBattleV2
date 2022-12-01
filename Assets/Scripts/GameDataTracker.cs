using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameDataTracker : MonoBehaviour
{
    public static GameDataTracker Instance;
    private Dictionary<int, float> roundTimeDic = new Dictionary<int, float>();
    private int playersKilledByHazard;
    private int totalRoundsPlayed;
    private float totalGameTime;
    private List<KillEvent> killList = new List<KillEvent>();
    private static string filePath = "Assets/Resources/GameLogs.txt";
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
    // Update is called once per frame
    void Update()
    {
      
    }
   

    public void NewKillEvent(int killer, int killed, string weaponName)
    {
        KillEvent killEvent = new KillEvent(killerID: killer, killedPlayerID: killed, weaponName);
        killList.Add(killEvent);
        print(killEvent.ToString());
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
        foreach(KillEvent eve in killList)
        {
            writer.WriteLine(eve.ToString());
        }
        writer.WriteLine();
        for(int i = 1; i <= totalRoundsPlayed; i++)
        {
            writer.WriteLine($"Round {i} lasted {roundTimeDic[i]} seconds");
            totalGameTime += roundTimeDic[i];
        }
        writer.WriteLine($"Rounds played: {totalRoundsPlayed} \nTotal time of session: {totalGameTime} seconds \nAverage time per round: {totalGameTime / totalRoundsPlayed}");
        writer.WriteLine($"\n total amount of players killed by hazards : {playersKilledByHazard} \n" +
            $"");
        writer.Close();
    }

    public void SaveRoundTime(int roundNr, float duration)
    {
        roundTimeDic[roundNr] = duration;
        totalRoundsPlayed++;
    }


}

public struct KillEvent
{
    private readonly int killerID;
    private readonly int killedPlayerID;
    private readonly string weaponName;

    public KillEvent(int killerID, int killedPlayerID, string weaponName)
    {
        this.killerID = killerID;
        this.killedPlayerID = killedPlayerID;
        this.weaponName = weaponName;
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
