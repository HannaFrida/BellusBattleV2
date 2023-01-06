using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Martin Wallmark
*/
public class Pendulum : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle = 60f;
    private Quaternion start, end;
    private float timePassed = 6;
    
    // Start is called before the first frame update
    void Start()
    {
        start = PendulumRotation(angle);
        end = PendulumRotation(-angle);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(timePassed * speed + Mathf.PI / 2) + 1f) / 2f);
    }

    Quaternion PendulumRotation(float angle)
    {
        Quaternion currentRotation = transform.rotation;
        float angleX = currentRotation.eulerAngles.x + angle;

        if(angleX > 180f)
        {
            angleX -= 360f;
        }
        else if( angleX < -180f)
        {
            angleX += 360;
        }

        currentRotation.eulerAngles = new Vector3(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, angleX);
        return currentRotation;
    }
    
}
