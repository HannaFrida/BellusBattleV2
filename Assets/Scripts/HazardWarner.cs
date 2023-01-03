using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardWarner : MonoBehaviour
{
    [SerializeField] private Image warningIcon;
    private SoundManager soundManager;
    private float blinkTimer;
    private float blinkTime = 0.3f;
    private bool isShowingWarning;

    private void Start()
    {
        //soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        DisplayWarning(false);
    }
    void Update()
    {
        if (isShowingWarning == true)
        {
            RunBlinkTimer();
        }
    }

    public void DisplayWarning(bool toggle)
    {
        if (warningIcon == null) return;

        isShowingWarning = toggle;
        warningIcon.enabled = toggle;
    }

    private void WarningBlink()
    {
        switch (warningIcon.color.a.ToString())
        {
            case "1":
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 0);
                break;
            case "0":
                warningIcon.color = new Color(warningIcon.color.r, warningIcon.color.g, warningIcon.color.b, 1);
                if (soundManager != null)
                {
                    SoundManager.Instance.HazardWarningSound();
                }
                break;
        }
        blinkTimer = 0f;
    }

    private void RunBlinkTimer()
    {
        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkTime)
        {
            WarningBlink();
            

        }
    }
}
