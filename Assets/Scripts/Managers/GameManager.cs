using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Cinemachine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IDataPersistenceManager
{

    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private Transform cameraTarget;
    private bool gameLoopFinished = false;
    public static GameManager Instance;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<GameObject> playersAlive = new List<GameObject>();
    [SerializeField] private SoundManager soundManager;
    private bool gameHasStarted;
    [SerializeField] private bool gameIsPaused;

    [Header("Points")]
    private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] private int scoreToWin;
    [SerializeField] private bool hasGivenScore;
    [SerializeField] private float giveScoreTimer;
    [SerializeField] private bool hasOnePlayerLeft;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;
    private int winnerID;

    [Header("Levels")]
    [SerializeField] WhichScenesListToPlay scenceToPlay;
    [SerializeField] WhichOrderToPlayScenes playingScenesOrder;
    [SerializeField] private string[] scenes;
    [SerializeField] private List<LevelDetails> levels = new List<LevelDetails>();
    [SerializeField] private float timeTillRestartGame;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject levelXPrefab;
    private int sceneCount;
    private enum WhichScenesListToPlay { ScenesFromBuild, ScenesFromList, ScenesFromBuildAndList };
    private enum WhichOrderToPlayScenes { Random, NumiricalOrder };
    private string nextLevel;
    [Header("UI")]
    [SerializeField] private GameObject welcomePanel;

    [Header("Transition")]
    [SerializeField] private float transitionTime = 5f;
    AsyncOperation asyncLoad;
    [SerializeField] Transition trans;
    static Vector2 pos1, pos2, pos3, pos4;
    private static Dictionary<int, GameObject> transPosDic = new Dictionary<int, GameObject>();
    private static Dictionary<int, Image> imageDic = new Dictionary<int, Image>();

    public List<string> scenesToChooseFrom = new List<string>();
    public List<string> scenesToRemove = new List<string>();

    public List<string> GetScencesList()
    {
        return scenesToChooseFrom;
    }

    public bool GameIsPaused
    {
        get => gameIsPaused;
    }

    public bool GameHasStarted
    {
        get => GameHasStarted;
    }
    private void OnLevelWasLoaded(int level)
    {
        cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;
        if (cameraTarget == null) cameraTarget = new GameObject("temp").transform;
        if (level != 0)
        {
            
            giveScoreTimer = 0f;
            gameHasStarted = true;
            playersAlive = new List<GameObject>(players);
            DeactivateMovement();

            //Array.Clear(targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets, 0, targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length);
            //SpawnPlayers();
        }
        RestorePLayer();
        
        


    }
    private void Awake()
    {
        if (Instance != null) Debug.LogError("Found more than one Game Manager in scene.");
        Instance = this;
        gameLoopFinished = false;
        DontDestroyOnLoad(this);
        AddScenesToPlay();

    }

    private void Start()
    {
        //targetGroup = GameObject.FindGameObjectWithTag("targets");
        //trans = Transition.Instance;
        //trans.gameObject.SetActive(false);
        pos1 = new Vector2(477f, 160f); 
        pos2 = new Vector2(892f, 160f);
        pos3 = new Vector2(1139f, 160f);
        pos4 = new Vector2(1386f, 160f); // x = 977 - -409
        DataPersistenceManager.Instance.LoadGame();
    }

    private void Update()
    {
        if (targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length < 1)  {
            targetGroup.AddMember(cameraTarget , 1, 5); 
        }else if((targetGroup.m_Targets[0].target == cameraTarget) && targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length < 2) ;
        else targetGroup.RemoveMember(cameraTarget);
        if (!gameHasStarted) return;
        CheckPlayersLeft();

        if (hasOnePlayerLeft && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }

    }
    private void OnApplicationQuit()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void PauseGame()
    {
        soundManager.HalfMusicVolume();
        gameIsPaused = true;
        Time.timeScale = 0; 
    }

    public void ResumeGame()
    {
        soundManager.FullMusicVolume();
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    private void AddScenesToPlay()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenesToRemove.Add("MainMenu");
        scenesToRemove.Add("The_End");
        scenesToRemove.Add("TransitionScene");
        LoadScenesList();
        if (SceneManager.GetActiveScene().buildIndex == 0) CreateLevelsUI();
    }

    public void AddPLayer(GameObject player)
    {
        if(welcomePanel != null) welcomePanel.SetActive(false);
        players.Add(player);
        targetGroup.AddMember(player.transform, 1, 5); //OBS GER ERROR!
    }
    public void RestorePLayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            targetGroup.RemoveMember(players[i].transform);
            targetGroup.AddMember(players[i].transform, 1, 5); //OBS GER ERROR!
            players[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        }
    }

    private void DeactivateMovement()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<DashAdvanced>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void ActivateMovement()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<DashAdvanced>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    public void PlayerDeath(GameObject deadPlayer)
    {
        playersAlive.Remove(deadPlayer);
        targetGroup.RemoveMember(deadPlayer.transform); //OBS GER ERROR!
    }



    private void ClearScore()
    {
        scoreDic.Clear();
    }



    public List<GameObject> GetAllPlayers()
    {
        return players;
    }

    private void CheckPlayersLeft()
    {
        if (playersAlive.Count <= 1)
        {
            hasOnePlayerLeft = true;
            soundManager.FadeOutMusic();
            soundManager.FadeOutHazard();
        }
        else if (playersAlive.Count > 1)
        {
            hasOnePlayerLeft = false;
        }
    }

    private void AddScore(GameObject winner) //TODO använd playerID istället för hela spelarobjektet
    {
        if (!scoreDic.ContainsKey(winner))
        {
            scoreDic[winner] = 1;
        }
        else
        {
            scoreDic[winner]++;
        }

    }

    public int GetScore(GameObject player)
    {
        return !scoreDic.ContainsKey(player) ? 0 : scoreDic[player];
    }

    public void SetPointsToWin(int value)
    {
        scoreToWin = value;
    }

    private void GiveScoreAfterTimer()
    {

        giveScoreTimer += Time.deltaTime;
        if (giveScoreTimer <= giveScoreTime) return;

        if (playersAlive.Count != 0)
        {
            
            GameObject winner = playersAlive[0];
            winnerID = winner.GetComponent<PlayerDetails>().playerID;
            AddScore(playersAlive[0]);
            hasGivenScore = true;
            if (GetScore(winner) == scoreToWin)
            {
                ClearScore();
                StartCoroutine(RestartGame());
                //Nån har vunnit!
                return;
            }
        }
        else
        {
            winnerID = 0;
            Debug.Log("Its a draaaaw!");
        }
        hasGivenScore = false;
        LoadNextScene();

    }


    public void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromList) CreateListOfScenesFromList();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuildAndList) { CreateListOfScenesFromBuild(); CreateListOfScenesFromList(); }
        foreach (string scene in scenesToRemove)
        {
            scenesToChooseFrom.Remove(scene);
        }
    }
    // ahhaa
    private void CreateListOfScenesFromBuild()
    {
        for (int i = 0; i < sceneCount; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            scenesToChooseFrom.Add(tempStr);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    private void CreateLevelsUI()
    {
        for (int i = 0; i < sceneCount - 2; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (i != 0)
            {
                GameObject g = Instantiate(levelXPrefab);
                g.transform.parent = content.transform;
                levels.Add(g.GetComponent<LevelDetails>());
                levels.ElementAt(i - 1).SetName(tempStr);
            }

        }
    }
    private void CreateListOfScenesFromList()
    {
        foreach (string scene in scenes)
        {
            scenesToChooseFrom.Add(scene);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in scences list or choose ScenesFromBuild from " + gameObject);
    }

    public void ChangeScenesToChooseFrom(LevelDetails scene)
    {
        if (scene.GetToggle() && scenesToChooseFrom.Count > 0)
        {
            scenesToChooseFrom.Remove(scene.GetName());
            scenesToRemove.Add(scene.GetName());
        }
        else
        {
            scenesToChooseFrom.Add(scene.GetName());
            scenesToRemove.Remove(scene.GetName());
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("TransitionScene");

        if (scenesToChooseFrom.Count <= 0)
        {
            Application.OpenURL("https://www.youtube.com/watch?v=WEEM2Qc9sUg");
            return;
        }
        if (playingScenesOrder == WhichOrderToPlayScenes.Random)
        {
            LoadNextSceneInRandomOrder();
        }

        else if (playingScenesOrder == WhichOrderToPlayScenes.NumiricalOrder)
        {
            LoadNextSceneInNumericalOrder();
        }
        
        if (scenesToChooseFrom.Count <= 0)
        {
            LoadScenesList();

        }
        StartCoroutine(AsynchronousLoad());
        soundManager.FadeInMusic();
    }
    /*
    private void LoadNextSceneInNumericalOrder()
    {
        SceneManager.LoadScene(scenesToChooseFrom.ElementAt(0));
        scenesToChooseFrom.RemoveAt(0);
    }
    private void LoadNextSceneInRandomOrder()
    {
        int randomNumber = Random.Range(0, scenesToChooseFrom.Count);
        SceneManager.LoadScene(scenesToChooseFrom.ElementAt(randomNumber));
        scenesToChooseFrom.RemoveAt(randomNumber);
    }
    */

    private string LoadNextSceneInNumericalOrder()
    {
        nextLevel = scenesToChooseFrom.ElementAt(0);
        scenesToChooseFrom.RemoveAt(0);
        return nextLevel;
    }
    private string LoadNextSceneInRandomOrder()
    {
        int randomNumber = Random.Range(0, scenesToChooseFrom.Count);
        nextLevel = scenesToChooseFrom.ElementAt(randomNumber);
        scenesToChooseFrom.RemoveAt(randomNumber);
        return nextLevel;
    }

    public void ReturnToMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        ReturnToLobby();
    }
    private IEnumerator RestartGame()
    {
        GameDataTracker.Instance.WriteToFile();
        SceneManager.LoadScene("The_End");
        gameLoopFinished = true;
        DataPersistenceManager.Instance.SaveGame();
        yield return new WaitForSeconds(timeTillRestartGame);
        Destroy(transform.parent.gameObject);
        SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
    }
    private void ReturnToLobby()
    {
        SceneManager.LoadScene("MainMenu");
        gameLoopFinished = true;
        DataPersistenceManager.Instance.SaveGame();
        //yield return new WaitForSeconds(timeTillRestartGame);
        Destroy(transform.parent.gameObject);
        SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
    }


    public int GetWinnerID()
    {
        return winnerID;
    }

    // The Application loads the Scene in the background as the current Scene runs.
    // transitionTime is how long the TransitionScene is shown before continuing
    private IEnumerator AsynchronousLoad()
    {
        yield return new WaitForSeconds(transitionTime);

        Application.backgroundLoadingPriority = ThreadPriority.High;

        asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (Mathf.Approximately(asyncLoad.progress, 0.9f))
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        yield return new WaitForEndOfFrame();
        yield return null;
    }

    public void MoveUpPlayer()
    {
        if (winnerID == 0)
        {
            Debug.Log("draw or something");
        }
        else if(winnerID == 1)
        {
            //Debug.Log(scoreDic[playersAlive[0]] + "");
            //Debug.Log(winnerID);
            //Debug.Log("winnah");
            imageDic[winnerID] = trans.getImage1;
            RectTransform picture1 = trans.getImage1.GetComponent<RectTransform>();
            picture1.transform.position = pos1;
            pos1 = new Vector2(picture1.position.x, picture1.position.y + 20);
            trans.getWinScore1.SetText(scoreDic[playersAlive[0]] + "");
        }
        else if (winnerID == 2)
        {
            //Debug.Log(scoreDic[playersAlive[0]] + "");
            //Debug.Log(winnerID);
            //Debug.Log("ahhhhhhh");
            imageDic[winnerID] = trans.getImage2;
            RectTransform picture2 = trans.getImage2.GetComponent<RectTransform>();
            picture2.transform.position = pos2;
            pos2 = new Vector2(picture2.position.x, picture2.position.y + 20);
            trans.getWinScore2.SetText(scoreDic[playersAlive[0]] + "");
        }
        else if (winnerID == 3)
        {
            imageDic[winnerID] = trans.getImage3;
            RectTransform picture3 = trans.getImage3.GetComponent<RectTransform>();
            picture3.transform.position = pos3;
            pos3 = new Vector2(picture3.position.x, picture3.position.y + 20);
            trans.getWinScore3.SetText(scoreDic[playersAlive[0]] + "");
        }
        else if (winnerID == 4)
        {
            imageDic[winnerID] = trans.getImage4;
            RectTransform picture4 = trans.getImage4.GetComponent<RectTransform>();
            picture4.transform.position = pos4;
            pos4 = new Vector2(picture4.position.x, picture4.position.y + 20);
            trans.getWinScore4.SetText(scoreDic[playersAlive[0]] + "");
        }
    }

    public void LoadData(GameData data)
    {
        players = data.players;
        playersAlive = new List<GameObject>(players);
        for(int i = 0; i< players.Count(); i++)
        {
            targetGroup.AddMember(players[i].transform, 1, 5);
        }

    }

    public void SaveData(ref GameData data)
    {
        if(!gameLoopFinished) players.Clear();
        data.players = players;
    }
}
