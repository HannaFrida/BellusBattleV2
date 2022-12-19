using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] protected EventSystem es;
    [SerializeField] protected GameObject buttonDeafaultPanel;
    [SerializeField] protected GameObject buttonMapsSelectionPanel;
    [SerializeField] private SoundManager soundManager;
    private List<string> actionMapNames = new();
    //[SerializeField] private List<Button> buttons = new List<Button>();
    protected GameObject activePanel;
    protected void Start()
    {
        if (es == null) es = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        for (int i = 0; i < panels.Count; i++) panels[i].SetActive(false);
        panels[0].SetActive(true);
        activePanel = panels[0];
        es.SetSelectedGameObject(buttonDeafaultPanel);
    }
    private void OnEnable()
    {
        if (activePanel == null) return;

        switch (activePanel.name)
        {
            case "Defualt_Panel": es.SetSelectedGameObject(buttonDeafaultPanel); break;
            case "MapsSelection_Panel": es.SetSelectedGameObject(buttonMapsSelectionPanel); break;
            default: es.SetSelectedGameObject(buttonDeafaultPanel); break;
        }
    }
    virtual public void SetPanelActive(GameObject panel)
    {
        panel.SetActive(true);
        activePanel.SetActive(false);
        activePanel = panel;
        switch(panel.name)
        {
            case "Defualt_Panel": es.SetSelectedGameObject(buttonDeafaultPanel); break;
            case "MapsSelection_Panel": es.SetSelectedGameObject(buttonMapsSelectionPanel); break;
        }
    }
    public void OpenTheSuprise()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }

    public void AddToList(string name)
    {
        actionMapNames.Add(name);
    }

    virtual public void ExitUI()
    {
        if (activePanel.name.Equals("MapsSelection_Panel"))
        {
            SetPanelActive(panels[0]);
            return;
        }
        else
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] == null) continue;
                //if (obj[i].GetComponent<PlayerInput>().currentActionMap.name.Equals("PlayerAccessibilityLeft")) break;
                obj[i].GetComponent<PlayerInput>().SwitchCurrentActionMap(obj[i].GetComponent<PlayerDetails>().ChosenActionMap);
            }
            actionMapNames.Clear();


            gameObject.SetActive(false);
            soundManager.FadeInBellSounds();
        }
    }
    public void NavigateRight()
    {
        if (panels.IndexOf(activePanel)+1 < panels.Count)
        {
            SetPanelActive(panels[panels.IndexOf(activePanel) + 1]);
        }
    }
    public void NavigateLeft()
    {
        if (panels.IndexOf(activePanel)-1 >= 0) SetPanelActive(panels[panels.IndexOf(activePanel) - 1]);

    }

}
