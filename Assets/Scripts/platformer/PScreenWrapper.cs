using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PScreenWrapper : MonoBehaviour
{
    float leftConstraint = Screen.width;
    float rightConstraint = Screen.width;
    float buffer = 1.0f;
    Camera cam;
    float distanceX;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        distanceX = Mathf.Abs(cam.transform.position.x + transform.position.x);
        leftConstraint = cam.ScreenToWorldPoint(new Vector2(distanceX, 0.0f)).x;
        rightConstraint = cam.ScreenToWorldPoint(new Vector2(Screen.width, distanceX)).x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x < leftConstraint - buffer)
        {
            transform.position = new Vector2(rightConstraint - 0.10f, transform.position.y);
        }
        if (transform.position.x > rightConstraint)
        {
            transform.position = new Vector2(leftConstraint - 0.10f, transform.position.y);
        }
    }
}
