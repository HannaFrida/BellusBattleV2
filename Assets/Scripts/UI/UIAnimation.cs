using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
* Author Khaled Alraas
*/
public class UIAnimation : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    public void OnSelect(BaseEventData eventData)
    {
        gameObject.transform.localScale= Vector3.one * 1.2f;
    }
    public void OnDeselect(BaseEventData data)
    {
        gameObject.transform.localScale = Vector3.one * 1f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
