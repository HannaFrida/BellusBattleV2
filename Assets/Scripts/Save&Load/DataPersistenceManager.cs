using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "temp.game";


    private GameData gameData;

    private List<IDataPersistenceManager> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null) Debug.LogError("Found more than one Game Persistence Manager in scene.");
        Instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

    }
    private void Start()
    {
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
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
    public void SaveGame()
    {
        foreach (IDataPersistenceManager dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        //Debug.Log(gameData.players.Count);

        dataHandler.Save(gameData);

    }

    private List<IDataPersistenceManager> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistenceManager> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>();
        return new List<IDataPersistenceManager>(dataPersistenceObjects);
    }
}
