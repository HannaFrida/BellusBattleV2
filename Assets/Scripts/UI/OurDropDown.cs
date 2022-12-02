using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OurDropDown : MonoBehaviour
{
    [SerializeField] private List<Button> elements = new List<Button>();
    [SerializeField] private Button navRight;
    [SerializeField] private Button navLeft;
    private Button activeButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var element in elements)
        {
            element.gameObject.SetActive(false);
        }
        elements[0].gameObject.SetActive(true);
        activeButton = elements[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnNavRight()
    {
        if(elements.IndexOf(activeButton) < elements.Count-1)SetActiveButton(elements[elements.IndexOf(activeButton) + 1]);
    }
    public void OnNavLeft()
    {
        if (elements.IndexOf(activeButton) > 0) SetActiveButton(elements[elements.IndexOf(activeButton) - 1]);

    }
    private void SetActiveButton(Button element)
    {
        activeButton.gameObject.SetActive(false);
        element.gameObject.SetActive(true);
        activeButton = element;
    }
    public string GetActiveElement()
    {
        return activeButton.gameObject.name;
    }
}
