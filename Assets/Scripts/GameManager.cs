using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            giveScoreTimer = 0f;
            gameHasStarted = true;
            playersAlive = new List<GameObject>(players);
        }

        //trans = Transition.Instance;
        /*
        if (SceneManager.GetActiveScene().name == "TransitionScene")
        {
            trans.gameObject.SetActive(true);
            MoveUpPlayer();
        }
        else
        {
            trans.gameObject.SetActive(false);
        }
        */
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        AddScenesToPlay();

    }

    private void Start()
    {
        //trans = Transition.Instance;
        //trans.gameObject.SetActive(false);
        pos1 = new Vector2(477f, 160f); 
        pos2 = new Vector2(892f, 160f);
        pos3 = new Vector2(1139f, 160f);
        pos4 = new Vector2(1386f, 160f); // x = 977 - -409
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

        foreach(GameObject player in players)
        {
            
        }

        if (playersAlive.Count != 0)
        {
            
            GameObject winner = playersAlive[0];
            winnerID = winner.GetComponent<PlayerDetails>().playerID;
            AddScore(playersAlive[0]);
            hasGivenScore = true;
            if (getScore(winner) == scoreToWin)
            {
                ClearScore();
                Finish(gameObject);
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

    public void MoveUpPlayer()
    {
        //GameObject winnah = transPosDic[winnerID];
        if (winnerID == 0)
        {
            Debug.Log("fuck oyu");
        }
        else if(winnerID == 1)
        {
            Debug.Log(winnerID);
            Debug.Log("winnah");
            imageDic[winnerID] = trans.getImage1;
            RectTransform picture1 = trans.getImage1.GetComponent<RectTransform>();
            picture1.transform.position = pos1;
            pos1 = new Vector2(picture1.position.x, picture1.position.y + 20);
        }
        else if (winnerID == 2)
        {
            Debug.Log(winnerID);
            Debug.Log("ahhhhhhh");
            imageDic[winnerID] = trans.getImage2;
            RectTransform picture2 = trans.getImage2.GetComponent<RectTransform>();
            //pos1 = picture.transform.position;
            picture2.transform.position = pos2;
            pos2 = new Vector2(picture2.position.x, picture2.position.y + 20);
        }
        else if (winnerID == 3)
        {
            imageDic[winnerID] = trans.getImage3;
            RectTransform picture3 = trans.getImage3.GetComponent<RectTransform>();
            //pos1 = picture.transform.position;
            picture3.transform.position = pos3;
            pos3 = new Vector2(picture3.position.x, picture3.position.y + 20);
        }
        else if (winnerID == 4)
        {
            imageDic[winnerID] = trans.getImage4;
            RectTransform picture4 = trans.getImage4.GetComponent<RectTransform>();
            //pos1 = picture.transform.position;
            picture4.transform.position = pos4;
            pos4 = new Vector2(picture4.position.x, picture4.position.y + 20);
        }


        //winner = gameManager.GetWinnerID();
        //image1.transform.position = timesTransitionHappen;
        //RectTransform picture = image1.GetComponent<RectTransform>();
        //pos1 = picture.transform.position;
        //picture.transform.position = pos1;
        //pos1 = new Vector2(picture.position.x, picture.position.y + 20);
        //picture.position = new Vector2(picture.position.x, picture.position.y + 20);
    }
}
