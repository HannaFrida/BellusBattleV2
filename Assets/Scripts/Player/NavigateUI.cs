using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigateUI : MonoBehaviour
{
    UIMenuHandler g;
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NavigateRight(InputAction.CallbackContext context)
    {
        g.NavigateRight();
    }
    public void NavigateLeft(InputAction.CallbackContext context)
    {
        g.NavigateLeft();
    }
    public void ExitUI(InputAction.CallbackContext context)
    {
        g.ExitUI();
    }
    public void SetConnection(GameObject gg)
    {
        g = gg.GetComponent<UIMenuHandler>();

    }
}
