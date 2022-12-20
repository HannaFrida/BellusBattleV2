using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = Application.dataPath;
    private string dataFileName = "temp.game";
    private string playerDataFileName = "player.game";
    private string hatsDataFileName = "hats.game";

    public FileDataHandler(string dataDirPath, string dataDileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataDileName;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data to fiel: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public PlayerData LoadPlayerData()
    {
        string fullPath = Path.Combine(dataDirPath, playerDataFileName);
        PlayerData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to fiel: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public HatsData LoadHatsData()
    {
        string fullPath = Path.Combine(dataDirPath, hatsDataFileName);
        HatsData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<HatsData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to fiel: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to fiel: " + fullPath + "\n" + e);
        }
    }
    public void SavePlayerData(PlayerData data)
    {
        string fullPath = Path.Combine(dataDirPath, playerDataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to fiel: " + fullPath + "\n" + e);
        }
    }
    public void SaveHatsdata(HatsData data)
    {
        string fullPath = Path.Combine(dataDirPath, hatsDataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to fiel: " + fullPath + "\n" + e);
        }
    }

}
