using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OurDropDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements = new List<GameObject>();
    [SerializeField] private Button navRight, navLeft;
    [SerializeField] private int startElement = 0;
    private GameObject activeButton;
    [SerializeField] private SettingsUIHandler settings;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var element in elements)
        {
            element.gameObject.SetActive(false);
        }
        elements[startElement].gameObject.SetActive(true);
        activeButton = elements[startElement].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnNavRight(string toDo)
    {
        if (elements.IndexOf(activeButton) < elements.Count - 1)
        {
            SetActiveButton(elements[elements.IndexOf(activeButton) + 1]);
            ChooseSettingTodo(toDo);
        }
    }
    public void OnNavLeft(string toDo)
    {
        if (elements.IndexOf(activeButton) > 0)
        {
            SetActiveButton(elements[elements.IndexOf(activeButton) - 1]);
            ChooseSettingTodo(toDo);
        }

    }
    private void SetActiveButton(GameObject element)
    {
        activeButton.gameObject.SetActive(false);
        element.gameObject.SetActive(true);
        activeButton = element;
    }
    private void ChooseSettingTodo(string toDo)
    {
        switch(toDo)
        {
            case "Quality":
                settings.SetQuality(GetActiveElementIndex());
                break;
            case "Resolution": 
                settings.SetResolution(GetActiveElementIndex());
                break;
        }
    }
    public int GetActiveElementIndex()
    {
        return elements.IndexOf(activeButton);
    }
}
