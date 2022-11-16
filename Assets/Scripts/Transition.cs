using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    Image image1; // Baloons
    Image image2; // Baloons
    GameManager gameManager;
    int winner;
    public static Transition Instance;

    private void Start()
    {
        //gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        
    }

    void MoveUpPlayer()
    {
        winner = gameManager.GetWinnerID();

    }
}
