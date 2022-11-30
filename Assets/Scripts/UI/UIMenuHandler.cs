using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private EventSystem eventSys;
    [SerializeField] private GameObject button;
    private GameObject activePanel;
    protected void Start()
    {
        eventSys = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        panels[0].SetActive(true);
        activePanel = panels[0];
    }
    public void SetPanelActive(GameObject panel)
    {
        panel.SetActive(true);
        activePanel.SetActive(false);
        activePanel = panel;
    }
    public void OpenTheSuprise()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
    public void ExitUI()
    {
        this.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }
    private void OnEnable()
    {
        eventSys.SetSelectedGameObject(button);
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
