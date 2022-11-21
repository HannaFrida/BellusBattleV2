using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BoneRotation : MonoBehaviour
{

    [SerializeField] private List<RotationObject> rotationObjects;
    public enum axisToRotate { x, y, z, }

    private float angle = 0;

    private bool isFacingRight, usingOverride;

    [SerializeField] private GameObject target;


    [SerializeField] private List<GameObject> flipObjects;
    [SerializeField] private float maxRotation = 90f;

    [System.Serializable]
    public struct RotationObject
    {
        public GameObject objectToRotate;
        public axisToRotate axis;
        [Tooltip("Determines how big influence this rotationobject has on the total rotation on the rig. max 180 degrees. Expected total sum of 1")]
        public float rotationWeight;
    }


    private void Update()
    {
        //CheckRotationFlip();
    }

    private void Start()
    {
        isFacingRight = false;
    }

    public void CalculateRotation(InputAction.CallbackContext context)
    {
        Vector2 joystickPosition = context.ReadValue<Vector2>();

        if (joystickPosition.x == 0 && joystickPosition.y == 0 || usingOverride) return;

        HandleRotation(joystickPosition);


    }

    //Override left stick aim with rightstick
    public void RightStickRotationOverride(InputAction.CallbackContext context)
    {
        Debug.Log("Overriding");
        Vector2 joystickPosition = context.ReadValue<Vector2>();
        if (joystickPosition.x == 0 && joystickPosition.y == 0)
        {
            usingOverride = false;
            return;
        }
        usingOverride = true;
        HandleRotation(joystickPosition);

    }

    void HandleRotation(Vector2 joystickPos)
    {
        if (!isFacingRight) joystickPos = new Vector3(joystickPos.x*-1, joystickPos.y);
        angle = Mathf.Atan2(joystickPos.y, joystickPos.x) * Mathf.Rad2Deg;
        if (angle > maxRotation && usingOverride || angle < -maxRotation && usingOverride) FlipObjects();
        angle = ClampRotation(angle);

        //Debug.Log(angle);

        for (int i = 0; i < rotationObjects.Count; i++)
        {

            float objectAngle = angle * rotationObjects[i].rotationWeight;
            //if (!isFacingRight) { objectAngle= -objectAngle; }
            Quaternion rot = Quaternion.AngleAxis(-objectAngle, SelectAxis(rotationObjects[i].axis));
            rotationObjects[i].objectToRotate.transform.localRotation = rot;
        }
    }

    private Vector3 SelectAxis(axisToRotate axis)
    {
        switch (axis)
        {
            case axisToRotate.x:
                return new Vector3(1, 0, 0);
            case axisToRotate.y:
                return new Vector3(0, 1, 0);
            case axisToRotate.z:
                return new Vector3(0, 0, 1);
        }
        return Vector3.zero;
    }

    public void CheckForFlip(InputAction.CallbackContext context)
    {
        Vector2 position = context.ReadValue<Vector2>();
        if (position.x > 0 && !isFacingRight)
        {
            FlipObjects();
        }
        else if (position.x < 0 && isFacingRight)
        {
            FlipObjects();
        }
    }


    private void FlipObjects()
    {
        if (usingOverride) { return; }
        if (!isFacingRight)
        {
            foreach (GameObject objectsToFlip in flipObjects) objectsToFlip.transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = true;
            Debug.Log("Flipped right");
        }
        else if (isFacingRight)
        {
            foreach (GameObject objectsToFlip in flipObjects) objectsToFlip.transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = false;
            Debug.Log("Flipped left");
        }

    }

    private float ClampRotation(float angleSum)
    {
        if (angleSum > maxRotation || angleSum < -maxRotation)
        {
            angleSum = Mathf.Clamp(angleSum, -maxRotation, maxRotation);
        }

        return angleSum;

    }


}
