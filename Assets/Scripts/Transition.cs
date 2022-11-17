using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] Image image1; // Baloons
    [SerializeField] Image image2; // Baloons
    [SerializeField] Image image3; // Baloons
    [SerializeField] Image image4; // Baloons
    GameManager gameManager;
    int winner;
    public static Transition Instance;
    int timesTransitionHappen;
    [SerializeField] GameObject panel;

    public Image getImage1 { get => image1; }
    public Image getImage2 { get => image2; }
    public Image getImage3 { get => image3; }
    public Image getImage4 { get => image4; }

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
        if (SceneManager.GetActiveScene().name == "TransitionScene")
        {
            panel.SetActive(true);
            gameManager.MoveUpPlayer();
        }
        else
        {
            panel.SetActive(false);
        }

    }

    private void Update()
    {
        
    }

    void MoveUpPlayer()
    {
        //winner = gameManager.GetWinnerID();
        //image1.transform.position = timesTransitionHappen;
        RectTransform picture = image1.GetComponent<RectTransform>();
        picture.position = new Vector2(picture.position.x, picture.position.y + 20);
    }
}
