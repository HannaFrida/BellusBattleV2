using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
* Author Hanna Rudöfors
*/
public class TimeManager : MonoBehaviour
{
    [SerializeField] private float slowdownAmount = 0.05f;
    [SerializeField] private bool isSlowMo;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI percentageText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        ChangeSliderValue();
        if (GameManager.Instance.GameIsPaused == true) return;

        Time.timeScale += 1f * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void FixedUpdate()
    {
        if (isSlowMo)
        {
            DoSlowMotion();
        }
    }

    private void DoSlowMotion()
    {
        Time.timeScale = slowdownAmount;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // The settings button for slow mo
    public void OnButtonPressed()
    {
        // invert the bool flag
        isSlowMo = !isSlowMo;
        DoSlowMotion();
    }

    // Used for adjusting amount of slowmo
    public void ChangeSliderValue()
    {
        if (slider == null) return;
        slowdownAmount = slider.value /10;

    }

    public void textUpdate(float value)
    {
        percentageText.text = value + "%";
    }

}
