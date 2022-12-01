using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenRebind : MonoBehaviour
{
    public RebindingDisplay rd;

    void Start()
    {

    }

    public void OnClick()
    {
        RebindingDisplay[] rebindingDisplays = (RebindingDisplay[])FindObjectsOfType(typeof(RebindingDisplay));
        rd = GameObject.FindGameObjectWithTag("Rebind").GetComponent<RebindingDisplay>();
        rd.isChangingSettings = !rd.isChangingSettings;


        foreach (RebindingDisplay rebindDisp in rebindingDisplays)
        {

            if (rd.isChangingSettings)
            {
                for (int i = 0; i < rebindDisp.panels.Count; i++)
                {
                    rebindDisp.panels[i].SetActive(true);
                }
                //rebindDisp.panels.gameObject.SetActive(true);
            }
            else
            {
                for (int i = 0; i < rebindDisp.panels.Count; i++)
                {
                    rebindDisp.panels[i].SetActive(false);
                }
                //rebindDisp.panels.gameObject.SetActive(false);
            }
        }

    }
}
