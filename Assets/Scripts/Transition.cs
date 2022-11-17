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

    private void Start()
    { 
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        timesTransitionHappen++;
       
    }
    

    private void OnLevelWasLoaded(int level)
    {
       // MoveUpPlayer();
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
