using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField] private CameraFocus camera;
    public static GameManager Instance;
    [SerializeField] private List<GameObject> players = new List<GameObject>();
    [SerializeField] private List<GameObject> playersAlive = new List<GameObject>();
    [SerializeField] private SoundManager soundManager;
    private bool gameHasStarted;

    [Header("Poängrelaterat")]
    private static Dictionary<GameObject, int> scoreDic = new Dictionary<GameObject, int>();
    [SerializeField] private int scoreToWin;
    [SerializeField] private bool hasGivenScore;
    [SerializeField] private float giveScoreTimer;
    [SerializeField] private bool hasOnePlayerLeft;
    [SerializeField, Tooltip("Amount of time until the last player alive recieves their score")] private float giveScoreTime;
    private int winnerID;

    [Header("LevelRelaterat")]
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
    [SerializeField] private float transitionTime = 5f;
    AsyncOperation asyncLoad;

    public List<string> scenesToChooseFrom = new List<string>();
    public List<string> scenesToRemove = new List<string>();
    public List<string> GetScencesList()
    {
        return scenesToChooseFrom;
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            giveScoreTimer = 0f;
            gameHasStarted = true;
            playersAlive = new List<GameObject>(players);
        }     

    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        AddScenesToPlay();

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!gameHasStarted) return;
        CheckPlayersLeft();

        if (hasOnePlayerLeft && !hasGivenScore && gameHasStarted)
        {
            GiveScoreAfterTimer();
        }           

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
        players.Add(player);
    }

    public void PlayerDeath(GameObject deadPlayer)
    {
        playersAlive.Remove(deadPlayer);
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

    public int getScore(GameObject player)
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
            AddScore(playersAlive[0]);
            hasGivenScore = true;
            if (getScore(winner) == scoreToWin)
            {
                winnerID = winner.GetComponent<PlayerDetails>().playerID;
                ClearScore();
                Finish(gameObject);
                //Nån har vunnit!
                return;
            }
        }
        else
        {
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
        for (int i = 0; i < sceneCount - 1; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (i != 0)
            {
                Debug.Log("hahahah");
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
        if (playingScenesOrder == WhichOrderToPlayScenes.Random) LoadNextSceneInRandomOrder();
        else if (playingScenesOrder == WhichOrderToPlayScenes.NumiricalOrder) LoadNextSceneInNumericalOrder();
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
    public void Finish(GameObject destroyMe)
    {
        SceneManager.LoadScene("The_End");
        StartCoroutine(RestartGame(destroyMe));
    }
    private IEnumerator RestartGame(GameObject destroyMe)
    {
        yield return new WaitForSeconds(timeTillRestartGame);
        Destroy(destroyMe);
        Destroy(gameObject);
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
}
