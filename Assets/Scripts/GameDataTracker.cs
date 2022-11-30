using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataTracker : MonoBehaviour
{
    public static GameDataTracker Instance;
    private Dictionary<int, float> roundTimeDic = new();
    private int playersKilledByHazard;
    private int totalRoundsPlayed;
    private float totalGameTime;
    private List<KillEvent> killList = new List<KillEvent>();
    // Start is called before the first frame update

    private void Awake()
    {
        Debug.Log("yo world");
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
        return $"Player {killerID} killed Player {killedPlayerID} with the {weaponName}";
    }


}
