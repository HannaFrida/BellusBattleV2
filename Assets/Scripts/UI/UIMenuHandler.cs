using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private EventSystem eventSys;
    [SerializeField] private GameObject button;
    private GameObject activePanel;
    private void Start()
    {
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
    }
    private void OnEnable()
    {
        eventSys.SetSelectedGameObject(button);
    }

}
