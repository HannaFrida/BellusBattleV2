using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITransitionStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FactText;
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] killsTexts;
    [SerializeField] private GameObject[] playerIcons;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTransitionScene();
        ChooseInterestingStat();
    }

    private void UpdateTransitionScene()
    {
        for(int i = 0; i < GameManager.Instance.GetAllPlayers().Count; i++)
        {
            playerIcons[i].SetActive(true);
            scoreTexts[i].text = "Score: " + GameDataTracker.Instance.GetPlayerScore(i + 1);
            killsTexts[i].text = "Kills: " + GameDataTracker.Instance.GetPlayerKills(i + 1);
        }
    }

    private void ChooseInterestingStat()
    {
        string stat = "";

        if(GameDataTracker.Instance.MultiKillFinder().Equals("nothing interesting") == false)
        {
            stat = GameDataTracker.Instance.MultiKillFinder();
        }
        if (GameDataTracker.Instance.StreakFinder().Equals("nothing interesting") == false)
        {
            if(stat.Length == 0)
            {
                stat = GameDataTracker.Instance.StreakFinder();
            }
        }
        
        FactText.text = stat;
    }

}
