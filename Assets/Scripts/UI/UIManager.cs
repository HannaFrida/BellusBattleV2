using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private EventSystem es;
    private GameObject activePanel;

    protected void Start()
    {
        if(es == null) es = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        for(int i = 0; i < panels.Count; i++) panels[i].SetActive(false);
        panels[0].SetActive(true);
        activePanel = panels[0];
    }
    public void GoTo(GameObject panel)
    {
        panel.SetActive(true);
        activePanel.SetActive(false);
        activePanel = panel;
    }
    public void ExitPanel()
    {
        GoTo(panels[0]);
    }
}
