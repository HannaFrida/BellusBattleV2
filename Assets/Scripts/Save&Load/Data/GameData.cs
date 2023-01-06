using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Khaled Alraas
*/
[System.Serializable]
public class GameData
{
    public List<GameObject> players; 
    public List<GameObject> availableHats; 
    public List<GameObject> removedHats; 
    public GameData() {
        players = new List<GameObject>();
        availableHats = new List<GameObject>();
        removedHats = new List<GameObject>();
    }
}
[System.Serializable]
public class PlayerData
{
    public List<GameObject> players;
    public PlayerData() 
    {
        players = new List<GameObject>();
    }
}
[System.Serializable]
public class HatsData
{
    public List<GameObject> availableHats;
    public List<GameObject> removedHats;
    public HatsData()
    {
        availableHats= new List<GameObject>();
        removedHats= new List<GameObject>();
    }
}
