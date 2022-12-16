using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireworkPlayer : MonoBehaviour
{
    [SerializeField] private List<VisualEffect> allFireWorks = new();
    private SoundManager soundManager;
    private float timer = 1;
    private bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

            if (timer <= 0)
            {
            
            allFireWorks[Random.Range(0, allFireWorks.Count)].Play(); 
            soundManager.FireWorkSound();
            timer = Random.Range(0.5f, 1.5f);
                

        }
            
            
    }
}
