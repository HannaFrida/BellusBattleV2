using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private EventSystem es;
    [SerializeField] private GameObject buttonDeafaultPanel;
    [SerializeField] private GameObject buttonMapsSelectionPanel;
    [SerializeField] private SoundManager soundManager;
    //[SerializeField] private List<Button> buttons = new List<Button>();
    private GameObject activePanel;
    protected void Start()
    {
        if (es == null) es = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        for (int i = 0; i < panels.Count; i++) panels[i].SetActive(false);
        panels[0].SetActive(true);
        activePanel = panels[0];
    }
    public void SetPanelActive(GameObject panel)
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

    public void ExitUI()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < obj.Length; i++)
        {
            obj[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        }

        //foreach(Button b in buttons)
        //{
        //    Debug.Log(b.spriteState);
        //}
        gameObject.SetActive(false);
        soundManager.FadeInBellSounds();
    }
    private void OnEnable()
    {
        es.SetSelectedGameObject(buttonDeafaultPanel);
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
