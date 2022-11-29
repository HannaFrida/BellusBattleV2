using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetAllBindings : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    [SerializeField] private GameObject RebindPanel;
    private bool isActivePanel;

    public void ResetBindings()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    public void OnRebind(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            isActivePanel = !isActivePanel;
            RebindPanel.SetActive(isActivePanel);
        }
    }

}
