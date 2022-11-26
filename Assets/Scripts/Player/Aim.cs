using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    enum AngleRotations{ FullAngleRotation, HalvAngleRotation, FixedAnglesRotation }
    [SerializeField] private AngleRotations rotations;
    [SerializeField] private AngleRotations rotationsOverride;
    [Range (1, 64)][SerializeField] private int amountOfFixedAgnles;
    private const float FULLCIRCLE = 360f;
    private const float HALFCIRCLE = 180f;
    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion rotation;
    private float angle;
    private bool usingOverride = false;

    private void Update()
    {
        //MouseInputToAngleCalculation();
        transform.rotation = rotation;
    }
    private void MouseInputToAngleCalculation()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        direction = mousePos - transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
    public void DefualtJoystickInputToAngleCalculation(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameIsPaused == true) return;

        Vector2 t = context.ReadValue<Vector2>();
        if (t.x == 0 && t.y == 0 || usingOverride) return;
        direction.Normalize();
        angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg; // -90 degrees
        ChooseAngleRotation(rotations);
    }
    public void OverrideJoystickInputToAngleCalculation(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameIsPaused == true) return;

        Vector2 t = context.ReadValue<Vector2>();
        if (t.x == 0 && t.y == 0)
        {
            usingOverride = false;
            return;
        }
        usingOverride = true;
        direction = t - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(t.y, t.x) * Mathf.Rad2Deg; // -90 degrees
        ChooseAngleRotation(rotationsOverride);
    }
    private void ChooseAngleRotation(AngleRotations type)
    {
        switch (type)
        {
            case AngleRotations.FullAngleRotation:
                FullAngleRotation();
                break;
            case AngleRotations.HalvAngleRotation:
                HalvAngleRotation();
                break;
            case AngleRotations.FixedAnglesRotation:
                FixedAnglesRotation(amountOfFixedAgnles);
                break;
            default: break;
        }
    }
    private void FullAngleRotation()
    {
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void HalvAngleRotation()
    {
        if (angle > 90) rotation = Quaternion.AngleAxis(90, Vector3.forward);
        else if (angle < -90) rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        else rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void FixedAnglesRotation(float amount)
    {
        float x = FULLCIRCLE / amount;
        float y = x / 2;
        float z = HALFCIRCLE - y;
        if ((angle >= -HALFCIRCLE && angle < -z) || (angle >= z && angle < HALFCIRCLE))
        {
            rotation = Quaternion.AngleAxis(-180, Vector3.forward);
            return;
        }
        for (float i = -z; i < z; i += x)
        {
            if (angle >= i && angle < i + x)
            {
                rotation = Quaternion.AngleAxis(i + y, Vector3.forward);
            }
        }
    }
}
