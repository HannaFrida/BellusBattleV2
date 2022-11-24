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
                rebindDisp.panel.gameObject.SetActive(true);
            }
            else
            {
                rebindDisp.panel.gameObject.SetActive(false);
            }
        }
        
    }
}
