using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<GameObject> players; 
    public GameData() {
        players = new List<GameObject>();
    }
}
