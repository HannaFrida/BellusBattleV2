using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningUIManager : MonoBehaviour
{
    //private ScoreManager scoreManager;
    [SerializeField] private GameObject[] panels;
    // Start is called before the first frame update
    void Start()
    {
        //scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        for(int i = 0; i < panels.Length; i++)
        {
            if(GameManager.Instance.GetWinnerID() == i+1)
            {
                panels.ElementAt(i).gameObject.SetActive(true);
            }
            else
            {
                panels.ElementAt(i).gameObject.SetActive(false);
            }
        }
    }
}
