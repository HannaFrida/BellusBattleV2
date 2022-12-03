using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class GameDataTracker : MonoBehaviour
{

    public static GameDataTracker Instance;
    private Dictionary<int, float> roundTimeDic = new Dictionary<int, float>();
    private List<KillEvent> killList = new List<KillEvent>();
    private int playersKilledByHazard;
    private int totalRoundsPlayed;
    private float totalGameTime;
    private string filePath; 
    private bool isInEditor;
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
        return null;
        
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
        if(isInEditor == true)
        {
            WriteToFileEditor();
        }
        else
        {
            WriteDebugDevBuild();
        }
      
    }

    private void WriteToFileEditor()
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

    private void WriteDebugDevBuild()
    {
        Debug.Log($"Session completed at {System.DateTime.Now} ---------------------------------------------");
        foreach (KillEvent eve in killList)
        {
            Debug.Log(eve.ToString());
        }
        Debug.Log("");
        for (int i = 1; i <= totalRoundsPlayed; i++)
        {
            Debug.Log($"Round {i} lasted {roundTimeDic[i]} seconds");
            totalGameTime += roundTimeDic[i];
        }
        Debug.Log($"Rounds played: {totalRoundsPlayed} \nTotal time of session: {totalGameTime} seconds \nAverage time per round: {totalGameTime / totalRoundsPlayed} seconds");
        Debug.Log($"\ntotal amount of players killed by hazards : {playersKilledByHazard} \n" +
            $"");
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

    public KillEvent(int killerID, int killedPlayerID, string weaponName)
    {
        this.killerID = killerID;
        this.killedPlayerID = killedPlayerID;
        if (weaponName == null)
        {
            weaponName = "Unnamed hazard";
        }
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
