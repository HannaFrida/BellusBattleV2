using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI textS;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Animator animator;
    private Slider slider;
    private int nmrOfLevels;

    // Start is called before the first frame update
    void Start()
    {
        nmrOfLevels = 5;
        /*
        slider = GetComponent<Slider>();
        slider.minValue = 1;
        slider.maxValue = levelManager.GetScencesList().Count;
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textS.text = nmrOfLevels.ToString();

    }

    public void Increase()
    {
        nmrOfLevels++;
    }

    public void Decrease()
    {
        if (nmrOfLevels == 1) return;

        nmrOfLevels--;
    }
    public void OnPlay()
    {
        GameManager.Instance.SetPointsToWin(nmrOfLevels);
        // So that the players can move once the game starts
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        }
        //PlaySceneChangeAnimation();
        StartCoroutine(Timer(1));
        //GameManager.Instance.LoadNextScene();
    }
    public void PlaySceneChangeAnimation()
    {
        animator.Play("pressed");
    }
    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.LoadNextScene();
    }
}
