using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/*
* Author Khaled Alraas
*/
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "temp.game";


    private GameData gameData;
    private PlayerData playerData;
    private HatsData hatsData;

    private List<IDataPersistenceManager> dataPersistenceObjects;
    private List<IDataPersistenceManagerPlayer> playerDataPersistenceObjects;
    private List<IDataPersistenceManagerHats> HatsDataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Found more than one Game Persistence Manager in scene.");
        Instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        this.playerDataPersistenceObjects = FindAllPlayerDataPersistenceObjects();
        this.HatsDataPersistenceObjects = FindAllHatsDataPersistenceObjects();

    }
    private void Start()
    {
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void NewPlayerdata()
    {
        playerData = new PlayerData();
    }
    public void NewHatsData()
    {
        this.hatsData = new HatsData();
    }
    public void LoadGame()
    {
        //TODO - load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();
        // if no data can loaded , initialize to a new game
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults. ");
            NewGame();
        }
        foreach(IDataPersistenceManager dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        //Debug.Log(gameData.players.Count);
    }

    public void LoadPlayerData()
    {
        //TODO - load any saved data from a file using the data handler
        this.playerData = dataHandler.LoadPlayerData();
        // if no data can loaded , initialize to a new game
        if (this.playerData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults. ");
            NewPlayerdata();
        }
        foreach (IDataPersistenceManagerPlayer dataPersistenceObj in playerDataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(playerData);
        }
        //Debug.Log(gameData.players.Count);
    }
    public void LoadHatsData()
    {
        //TODO - load any saved data from a file using the data handler
        this.hatsData = dataHandler.LoadHatsData();
        // if no data can loaded , initialize to a new game
        if (this.hatsData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults. ");
            NewHatsData();
        }
        foreach (IDataPersistenceManagerHats dataPersistenceObj in HatsDataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(hatsData);
        }
        //Debug.Log(gameData.players.Count);
    }
    public void SaveGame()
    {
        foreach (IDataPersistenceManager dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //Debug.Log(gameData.players.Count);

        dataHandler.Save(gameData);

    }
    public void SavePlayerData()
    {
        foreach (IDataPersistenceManagerPlayer dataPersistenceObj in playerDataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref playerData);
        }
        //Debug.Log(gameData.players.Count);

        dataHandler.SavePlayerData(playerData);

    }
    public void SaveHatsData()
    {
        foreach (IDataPersistenceManagerHats dataPersistenceObj in HatsDataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref hatsData);
        }
        //Debug.Log(gameData.players.Count);

        dataHandler.SaveHatsdata(hatsData);

    }

    private List<IDataPersistenceManager> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistenceManager> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>();
        return new List<IDataPersistenceManager>(dataPersistenceObjects);
    }
    private List<IDataPersistenceManagerPlayer> FindAllPlayerDataPersistenceObjects()
    {
        IEnumerable<IDataPersistenceManagerPlayer> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManagerPlayer>();
        return new List<IDataPersistenceManagerPlayer>(dataPersistenceObjects);
    }
    private List<IDataPersistenceManagerHats> FindAllHatsDataPersistenceObjects()
    {
        IEnumerable<IDataPersistenceManagerHats> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManagerHats>();
        return new List<IDataPersistenceManagerHats>(dataPersistenceObjects);
    }
}
