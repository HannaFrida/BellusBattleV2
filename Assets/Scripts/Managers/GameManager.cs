using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Cinemachine;
using TMPro;
using UnityEngine.InputSystem;
/*
* Authors Martin Wallmark, Hanna Rudöfors, Khaled Alraas
*/
public class GameManager : MonoBehaviour, IDataPersistenceManagerPlayer {

    [Header("Win or Draw Text")]
    [SerializeField] private GameObject winnerParent;
    private bool extraDrawTime = true;

    [Header("Camera")]
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private Transform cameraTarget;
    //MiniGame
    private bool isInMiniGame = false;
    private bool gameLoopFinished = false;

    public static GameManager Instance;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<GameObject> playersAlive = new List<GameObject>();
    private List<PlayerInput> inputs = new List<PlayerInput>();
    [SerializeField] private SoundManager soundManager;
    private bool gameHasStarted;
    [SerializeField] private bool gameIsPaused;

    private bool hasRunTransition = false;
    private bool runRoundTimer;
    private bool acceptPlayerInput = true;
    [SerializeField] private float roundDuration;
    [SerializeField] private float roundTimer;
    [SerializeField] private int roundCounter;

    [Header("Points")]
    private Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
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
    [SerializeField] private float timeTillRestartGame;
    private int sceneCount;
    private enum WhichScenesListToPlay { ScenesFromBuild, ScenesFromList, ScenesFromBuildAndList };
    private enum WhichOrderToPlayScenes { Random, NumiricalOrder };
    private string nextLevel;
    private List<string> scenesToChooseFrom = new List<string>();
    private List<string> scenesToRemove = new List<string>();

    [Header("Transition")]
    [SerializeField] private float transitionTime = 2f;
    AsyncOperation asyncLoad;
    [SerializeField] Transition trans;
   

    public bool _safeMode = false;

    
    
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

    public int GetScoreToWin
    {
        get => scoreToWin;
    }
    public void SetIsinMiniGame(bool value)
    {
        isInMiniGame = value;
    }
    private void OnLevelWasLoaded(int level)
    {
        SoundManager.Instance.FadeOutLavaHazard(0.1f);
        ValidatePlayerLists();

        if (level != 0)
        {
            acceptPlayerInput = false;
            giveScoreTimer = 0f;
            gameHasStarted = true;
            playersAlive = new List<GameObject>(players);
            ActivateMovement();
            DeactivateMovement();
        }

        if(SceneManager.GetActiveScene().name.Equals("TransitionScene") == false)
        {
            roundCounter++;
            GameDataTracker.Instance.SetCurrentRound(roundCounter);
            runRoundTimer = false;
            roundTimer = 0f;
            roundDuration = 0f;
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
        DataPersistenceManager.Instance.LoadPlayerData();
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

        if (hasOnePlayerLeft && !hasGivenScore && gameHasStarted && !isInMiniGame)
        {
            GiveScoreAfterTimer();
        }else if(playersAlive.Count <= 0)
        {
            StartCoroutine(WaitAndGoToLobby());
        }

        if (!runRoundTimer) return;
        if(acceptPlayerInput == false)
        {
            acceptPlayerInput = true;
        }
        roundTimer += Time.deltaTime;
        roundDuration = roundTimer;

    }
    IEnumerator WaitAndGoToLobby()
    {
        yield return new WaitForSeconds(giveScoreTime);
        ReturnToLobby();
    }

    /*
     * Author Martin Wallmark
     */
    private void ValidatePlayerLists()
    {
        if (players.Count == 0) return;

        players.RemoveAll(x => x == null);
    }
    private void OnApplicationQuit()
    {
        DataPersistenceManager.Instance.SavePlayerData();
    }

    /*
    * Author Martin Wallmark
    */
    public void PauseGame()
    {
        soundManager.HalfMusicVolume();
        gameIsPaused = true;
        Time.timeScale = 0; 
    }

    /*
    * Author Martin Wallmark
    */
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
        scenesToRemove.Add("Platformer");
        LoadScenesList();
    }

    public void AddPLayer(GameObject player)
    {
        players.Add(player);
        targetGroup.AddMember(player.transform, 1, 5); 
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
            targetGroup.AddMember(players[i].transform, 1, 5); 
        }
        
        foreach (GameObject player in players) {
            if (player.GetComponentInChildren<PlayerIndicatorFollow>() != null)
            {
                player.GetComponentInChildren<PlayerIndicatorFollow>().UnFollow();
            }
            
        }
    }

    /*
    * Author Martin Wallmark
    */
    private void DeactivateMovement()
    {
        foreach (GameObject player in players)
        {
            if (player == null) continue;
            player.GetComponent<DashAdvanced>().enabled = false;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.ResetForces();
            pm.enabled = false;
            
        }
    }

    /*
    * Author Martin Wallmark
    */
    public void ActivateMovement()
    {
        foreach (GameObject player in players)
        {
            if (player == null) continue;
            player.GetComponent<DashAdvanced>().enabled = true;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.enabled = true;
            //pm.ResetForces();
        }
    }

    public void PlayerDeath(GameObject deadPlayer) {

        if (deadPlayer.GetComponentInChildren<PlayerIndicatorFollow>() != null)
        {
            deadPlayer.GetComponentInChildren<PlayerIndicatorFollow>().Follow();
        }
        playersAlive.Remove(deadPlayer);
        targetGroup.RemoveMember(deadPlayer.transform); 
    }
    private void ClearScore()
    {
        scoreDic.Clear();
    }



    public List<GameObject> GetAllPlayers()
    {
        return players;
    }

    /*
    * Author Martin Wallmark
    */
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

    /*
    * Author Martin Wallmark
    */
    private void AddScore(GameObject winner) 
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

    /*
    * Author Martin Wallmark
    */
    public int GetScore(GameObject player)
    {
        return !scoreDic.ContainsKey(player) ? 0 : scoreDic[player];
    }

    /*
    * Author Martin Wallmark
    */
    public void SetPointsToWin(int value)
    {
        scoreToWin = value;
    }

    /*
    * Author Martin Wallmark
    */
    private void GiveScoreAfterTimer() {
        
        //Show when a draw occurs - Gregory 
        if (playersAlive.Count == 0) {
            if (extraDrawTime) {
                giveScoreTimer -= 1f;
                extraDrawTime = false;
            }
            winnerParent.SetActive(true);
        }

        giveScoreTimer += Time.deltaTime;
        if (giveScoreTimer >= giveScoreTime -1f && hasRunTransition == false)
        {
            hasRunTransition = true;
            TransitionManager.Instance.FadeOutCoroutine();
        }

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
                ClearScore();
                StartCoroutine(RestartGame());
                hasRunTransition = false;
                return;
            }
        }
        else
        {
            winnerID = 0;
            
            
            
            GameDataTracker.Instance.AddWinner(roundCounter, 0);
        }
        hasGivenScore = false;
        hasRunTransition = false;
        winnerParent.SetActive(false);
        extraDrawTime = true;
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
        if (scene.GetToggle() && scenesToChooseFrom.Count != 1)
        {
            scenesToChooseFrom.Remove(scene.GetName());
            scenesToRemove.Add(scene.GetName());
        }
        else
        {
            if (scenesToChooseFrom.Contains(scene.GetName())) return;
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
        SceneManager.LoadScene("The_End");
        soundManager.FadeInEndSceneSounds();
        gameLoopFinished = true;
        DataPersistenceManager.Instance.SavePlayerData();
        yield return new WaitForSeconds(timeTillRestartGame);
        soundManager.FadeOutEndSceneSounds();
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
        ClearScore();
        SceneManager.LoadSceneAsync("MainMenu");
        gameLoopFinished = true;
        DataPersistenceManager.Instance.SavePlayerData();
        Destroy(transform.parent.gameObject);
        SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)));
    }


    public int GetWinnerID()
    {
        return winnerID;
    }

    /*
     * Author Hanna Rudöfors
     */

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

    public void LoadData(PlayerData data)
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

    public void SaveData(ref PlayerData data)
    {
        if(!gameLoopFinished) players.Clear();
        data.players = players;
    }
}
