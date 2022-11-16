using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    // Start is called before the first frame update

    private float time;
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();    
            
    }

    private void OnLevelWasLoaded(int level)
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }
       
}
