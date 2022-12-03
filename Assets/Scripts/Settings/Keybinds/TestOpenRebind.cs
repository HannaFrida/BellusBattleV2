using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenRebind : MonoBehaviour
{
    public RebindingDisplay rd;
    [SerializeField]
    private GameObject[] players;
    private PlayerMovement[] playersMovement;

    void Start()
    {

    }

    public void OnClick()
    {
        RebindingDisplay[] rebindingDisplays = (RebindingDisplay[])FindObjectsOfType(typeof(RebindingDisplay));
        rd = GameObject.FindGameObjectWithTag("Rebind").GetComponent<RebindingDisplay>();
        rd.isChangingSettings = !rd.isChangingSettings;


        playersMovement = (PlayerMovement[])FindObjectsOfType(typeof(PlayerMovement));
        //playersMovement = GameObject.FindGameObjectsWithTag("Player");

        foreach (RebindingDisplay rebindDisp in rebindingDisplays)
        {

            if (rd.isChangingSettings)
            {
                for (int i = 0; i < rebindDisp.panels.Count; i++)
                {
                    rebindDisp.panels[i].SetActive(true);
                }
                /*
                foreach (PlayerMovement pm in playersMovement)
                {
                    rd.TurnOffMovement(pm);
                }
                */
                //rebindDisp.panels.gameObject.SetActive(true);
            }
            else
            {
                for (int i = 0; i < rebindDisp.panels.Count; i++)
                {
                    rebindDisp.panels[i].SetActive(false);
                }
                /*
                foreach (PlayerMovement pm in playersMovement)
                {
                    rd.TurnOnMovement(pm);
                }
                */
                //rebindDisp.panels.gameObject.SetActive(false);
            }
        }

    }
}
