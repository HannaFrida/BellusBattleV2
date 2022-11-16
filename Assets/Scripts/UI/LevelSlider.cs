using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI textS;
    [SerializeField] private ScoreManager scoreManager;
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
        GameManager.Instance.LoadNextScene();
    }
}
