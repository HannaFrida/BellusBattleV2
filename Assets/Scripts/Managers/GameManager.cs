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
    private List<PlayerInput> inputs = new List<PlayerInput>();
    [SerializeField] private SoundManager soundManager;
    private bool gameHasStarted;
    [SerializeField] private bool gameIsPaused;

    private bool runRoundTimer;
    private bool acceptPlayerInput = true;
    [SerializeField] private float roundDuration;
    [SerializeField] private float roundTimer;
    [SerializeField] private int roundCounter;

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
    //static Vector2 pos1, pos2, pos3, pos4;
    //private static Dictionary<int, GameObject> transPosDic = new Dictionary<int, GameObject>();
    //private static Dictionary<int, Image> imageDic = new Dictionary<int, Image>();

    [SerializeField] private Image player1Dead;
    [SerializeField] private Image player1Alive;
    
    [SerializeField] private Image player2Dead;
    [SerializeField] private Image player2Alive;
    
    [SerializeField] private Image player3Dead;
    [SerializeField] private Image player3Alive;
    
    [SerializeField] private Image player4Dead;
    [SerializeField] private Image player4Alive;

    public bool _safeMode = false;

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

    public bool AcceptPlayerInput
    {
        get => acceptPlayerInput;
    }

    public bool GameHasStarted
    {
        get => GameHasStarted;
    }

    public bool IsRunningRoundTimer
    {
        get => runRoundTimer;
        set => runRoundTimer = value;
    }

    public float RoundDuration
    {
        get => roundDuration;
    }
    private void OnLevelWasLoaded(int level)
    {
        /*
        if(SceneManager.GetSceneAt(level).name.Equals("TranisitionScene") == false)
        {
            
        }
        */
        ValidatePlayerLists();

        if (level != 0)
        {
            acceptPlayerInput = false;
            giveScoreTimer = 0f;
            gameHasStarted = true;
            playersAlive = new List<GameObject>(players);
            DeactivateMovement();

            //Array.Clear(targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets, 0, targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length);
            //SpawnPlayers();

            /*
            // Used to prevent Ghost bullets 
            foreach (GameObject player in playersAlive)
            {
                if (player.GetComponentInChildren<Gun>() != null)
                {
                    player.GetComponentInChildren<Gun>().Drop();
                }
                
            }
            */
        }

        if(SceneManager.GetActiveScene().name.Equals("TransitionScene") == false)
        {
            roundCounter++;
            GameDataTracker.Instance.SetCurrentRound(roundCounter);
            runRoundTimer = false;
            roundTimer = 0f;
            roundDuration = 0f;
            ResetPlayerImage();
            RestorePLayer();
        }

        if (level == 0)
        {
            
            acceptPlayerInput = true;
            roundCounter = 0;
            GameDataTracker.Instance.SetCurrentRound(0);
        }
        

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
        cameraTarget = GameObject.FindGameObjectWithTag("CameraTarget").transform;  
        //targetGroup = GameObject.FindGameObjectWithTag("targets");
        //trans = Transition.Instance;
        //trans.gameObject.SetActive(false);
        //pos1 = new Vector2(477f, 160f); 
        //pos2 = new Vector2(892f, 160f);
        //pos3 = new Vector2(1139f, 160f);
        //pos4 = new Vector2(1386f, 160f); // x = 977 - -409
        DataPersistenceManager.Instance.LoadGame();
    }

    private void Update()
    {
        if (targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length < 1)
        {
            targetGroup.AddMember(cameraTarget, 1, 5);
        }
        // ska vara tom, annars freakar kameran
        else if ((targetGroup.m_Targets[0].target == cameraTarget) && targetGroup.GetComponent<CinemachineTargetGroup>().m_Targets.Length < 2);
        else targetGroup.RemoveMember(cameraTarget);
        if (!gameHasStarted) return;
        CheckPlayersLeft();

        if (hasOnePlayerLeft && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }

        if (!runRoundTimer) return;
        if(acceptPlayerInput == false)
        {
            acceptPlayerInput = true;
        }
        roundTimer += Time.deltaTime;
        roundDuration = roundTimer;

    }

    private void ValidatePlayerLists()
    {
        if (players.Count == 0) return;

        players.RemoveAll(x => x == null);
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
    }

    public void AddPLayer(GameObject player)
    {
        if(welcomePanel != null) welcomePanel.SetActive(false);
        players.Add(player);
        targetGroup.AddMember(player.transform, 1, 5); //OBS GER ERROR!
    }

    public void AddInput(PlayerInput input)
    {
        inputs.Add(input);
    }

    public List<PlayerInput> GetInputs()
    {
        return inputs;
    }
    public void RestorePLayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null) continue;
            targetGroup.RemoveMember(players[i].transform);
            targetGroup.AddMember(players[i].transform, 1, 5); //OBS GER ERROR!
        }
        
        foreach (GameObject player in players) {
            player.GetComponentInChildren<PlayerIndicatorFollow>().UnFollow();
        }
    }

    private void DeactivateMovement()
    {
        foreach (GameObject player in players)
        {
            if (player == null) continue;
            player.GetComponent<DashAdvanced>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void ActivateMovement()
    {
        foreach (GameObject player in players)
        {
            if (player == null) continue;
            player.GetComponent<DashAdvanced>().enabled = true;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.enabled = true;
            pm.ResetForces();





        }
    }

    public void PlayerDeath(GameObject deadPlayer) {
        
        deadPlayer.GetComponentInChildren<PlayerIndicatorFollow>().Follow();
        
        playersAlive.Remove(deadPlayer);
        targetGroup.RemoveMember(deadPlayer.transform); //OBS GER ERROR!
        SetDeathImage(deadPlayer.GetComponent<PlayerDetails>().playerID);
        
        
    }

    private void SetDeathImage(int playerID)
    {
        if(playerID == 1)
        {
            player1Dead.enabled = true;
            player1Alive.enabled = false;
        }
        if(playerID == 2)
        {
            player2Dead.enabled = true;
            player2Alive.enabled = false;
        }
        if (playerID == 3)
        {
            player3Dead.enabled = true;
            player3Alive.enabled = false;
        }
        if (playerID == 4)
        {
            player4Dead.enabled = true;
            player4Alive.enabled = false;
        }
    }

    private void ResetPlayerImage()
    {
        player1Dead.enabled = false;
        player1Alive.enabled = true;

        player2Dead.enabled = false;
        player2Alive.enabled = true;

        player3Dead.enabled = false;
        player3Alive.enabled = true;

        player4Dead.enabled = false;
        player4Alive.enabled = true;
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
            scoreDic.Add(winner, 1);
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
        GameDataTracker.Instance.SaveRoundTime(roundCounter, roundDuration);
        
        if (playersAlive.Count != 0)
        {
            
            GameObject winner = playersAlive[0];
            winnerID = winner.GetComponent<PlayerDetails>().playerID;
            GameDataTracker.Instance.AddWinner(roundCounter, winnerID);
            AddScore(playersAlive[0]);
            hasGivenScore = true;
            if (GetScore(winner) == scoreToWin)
            {
                //Debug.Log(GameDataTracker.Instance.GetScoreInOrder()[0] + ", " + GameDataTracker.Instance.GetScoreInOrder()[1] + ", " + GameDataTracker.Instance.GetScoreInOrder()[2]);
                ClearScore();
                StartCoroutine(RestartGame());
                //Nån har vunnit!
                return;
            }
        }
        else
        {
            winnerID = 0;
            GameDataTracker.Instance.AddWinner(roundCounter, 0);
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
        if(nextLevel != null)StartCoroutine(AsynchronousLoad());
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
        //GameDataTracker.Instance.WriteToFile();
        SceneManager.LoadScene("The_End");
        gameLoopFinished = true;
        DataPersistenceManager.Instance.SaveGame();
        yield return new WaitForSeconds(timeTillRestartGame);
        Destroy(transform.parent.gameObject);
        SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
    }
    private void ReturnToLobby()
    {
        foreach(GameObject player in players)
        {
            if (player.GetComponentInChildren<Gun>() != null)
            {
                player.GetComponentInChildren<Gun>().Drop();
            }
            
        }
        SceneManager.LoadSceneAsync("MainMenu");
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
        /*
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
        */
    }

    public void LoadData(GameData data)
    {
        players = data.players;
        playersAlive = new List<GameObject>(players);
        for(int i = 0; i< players.Count(); i++)
        {
            if(players[i] != null)
            {
                targetGroup.AddMember(players[i].transform, 1, 5);
            }
            
        }

    }

    public void SaveData(ref GameData data)
    {
        if(!gameLoopFinished) players.Clear();
        data.players = players;
    }
}
