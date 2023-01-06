using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
* Author Khaled Alraas
*/
public class NavigateUI : MonoBehaviour
{
    UIMenuHandler g;
    int x = 0;
    int y = 0;
    int z = 0;
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NavigateRight(InputAction.CallbackContext context)
    {
        if (g == null) return;
        if (x == 0)
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
        if (g == null) return;
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
        if (g == null) return; 

        if (z == 0)
        {
            g.ExitUI();
            z++;
        }
        else
        {
            z = 0;
        }
    }
    public void SetConnection(GameObject gg)
    {
        g = gg.GetComponent<UIMenuHandler>();
    }
}
