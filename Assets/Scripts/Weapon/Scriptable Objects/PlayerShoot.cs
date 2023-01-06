using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/*
* Author Hanna Rud�fors
*/
public class PlayerShoot : MonoBehaviour
{
    public Action shootInput;
    public Action dropInput;

    public void OnWeaponShoot(InputAction.CallbackContext ctx)
    {
        shootInput?.Invoke();
    }

    public void OnWeaponDrop(InputAction.CallbackContext ctx)
    {
        dropInput?.Invoke();
    }
}
