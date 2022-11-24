using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpenRebind : MonoBehaviour
{
    public RebindingDisplay rd;

    void Start()
    {
        rd = GameObject.FindGameObjectWithTag("Rebind").GetComponent<RebindingDisplay>();
    }

    public void OnClick()
    {
        rd = GameObject.FindGameObjectWithTag("Rebind").GetComponent<RebindingDisplay>();
        rd.isChangingSettings = !rd.isChangingSettings;
        if (rd.isChangingSettings)
        {
            rd.panel.gameObject.SetActive(true);
        }
        else
        {
            rd.panel.gameObject.SetActive(false);
        }
    }
}
