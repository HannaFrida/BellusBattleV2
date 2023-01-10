using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireworkPlayer : MonoBehaviour
{
    [SerializeField] private List<VisualEffect> allFireWorks = new();
    private float timer = 1;
  
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            allFireWorks[Random.Range(0, allFireWorks.Count)].Play();
            SoundManager.Instance.FireWorkSound();
            timer = Random.Range(0.5f, 1.5f);
        }      
    }
}
