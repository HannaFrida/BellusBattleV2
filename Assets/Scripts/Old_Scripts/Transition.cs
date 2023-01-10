using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Transition : MonoBehaviour
{
    [SerializeField] Image image1; // Baloons
    [SerializeField] Image image2; // Baloons
    [SerializeField] Image image3; // Baloons
    [SerializeField] Image image4; // Baloons

    [SerializeField] TextMeshProUGUI winScore1; // scrores
    [SerializeField] TextMeshProUGUI winScore2; // scrores
    [SerializeField] TextMeshProUGUI winScore3; // scrores
    [SerializeField] TextMeshProUGUI winScore4; // scrores

    GameManager gameManager;
    int winner;
    public static Transition Instance;
    int timesTransitionHappen;
    [SerializeField] GameObject panel;

    public Image GetImage1 { get => image1; }
    public Image GetImage2 { get => image2; }
    public Image GetImage3 { get => image3; }
    public Image GetImage4 { get => image4; }

    public TextMeshProUGUI GetWinScore1 { get => winScore1; }
    public TextMeshProUGUI GetWinScore2 { get => winScore2; }
    public TextMeshProUGUI GetWinScore3 { get => winScore3; }
    public TextMeshProUGUI GetWinScore4 { get => winScore4; }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameManager = GameManager.Instance;
        timesTransitionHappen++;

        
        Instance = this;
        panel.SetActive(false);
    }
    

    private void OnLevelWasLoaded(int level)
    {
        // MoveUpPlayer();
        /*
        if (SceneManager.GetActiveScene().name == "TransitionScene")
        {
            panel.SetActive(true);
            gameManager.MoveUpPlayer();
        }
        else
        {
            panel.SetActive(false);
        }*/

    }



}
