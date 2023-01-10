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
    private int nmrOfLevels;

    // Start is called before the first frame update
    void Start()
    {
        nmrOfLevels = 5;
    }
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
        StartCoroutine(Timer(1));
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
