using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistenceManager
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
public interface IDataPersistenceManagerPlayer
{
    void LoadData(PlayerData data);
    void SaveData(ref PlayerData data);
}
public interface IDataPersistenceManagerHats
{
    void LoadData(HatsData data);
    void SaveData(ref HatsData data);
}
