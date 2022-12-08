using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITransitionStats : MonoBehaviour
{
    TextMeshProUGUI FactText;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] killsTexts;
    GameDataTracker gameDataTracker;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        UpdateKillText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScoreText()
    {
        for(int i = 0; i < GameManager.Instance.GetAllPlayers().Count; i++)
        {
            scoreTexts[i].text = "Score: " + GameDataTracker.Instance.GetPlayerScore(i + 1);
        }
    }

    private void UpdateKillText()
    {
        for (int i = 0; i < GameManager.Instance.GetAllPlayers().Count; i++)
        {
            killsTexts[i].text = "Kills: " + GameDataTracker.Instance.GetPlayerKills(i+1);
        }
    }
}
