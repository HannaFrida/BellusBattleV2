using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavigateUI : MonoBehaviour
{
    UIMenuHandler g;
    int x = 0;
    int y = 0;
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NavigateRight(InputAction.CallbackContext context)
    {
        if(x == 0)
        {
            g.NavigateRight();
            x++;
        }
        else
        {
            x = 0;
        }
    }
    public void NavigateLeft(InputAction.CallbackContext context)
    {
        if (y == 0)
        {
            g.NavigateLeft();
            y++;
        }
        else
        {
            y = 0;
        }
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
