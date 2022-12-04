using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Flip : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    [SerializeField] private GameObject lookAtCtrl;
    private Vector3 lookAtCtrlPosition;
    bool facingRight = false;

    private PlayerMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.Velocity.x > 0)
        {
            gameObj.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            if (!facingRight)
            {
                lookAtCtrlPosition = lookAtCtrl.transform.localPosition;
                lookAtCtrl.transform.localPosition = new Vector3(lookAtCtrlPosition.x, lookAtCtrlPosition.y, -lookAtCtrlPosition.z);

                facingRight= true;
            }
            
        }
        else if (movement.Velocity.x < 0)
        {
            gameObj.transform.rotation = Quaternion.AngleAxis(0, Vector3.up);

            if (facingRight)
            {
                lookAtCtrlPosition = lookAtCtrl.transform.localPosition;
                lookAtCtrl.transform.localPosition = new Vector3(lookAtCtrlPosition.x, lookAtCtrlPosition.y, Mathf.Abs(lookAtCtrlPosition.z));
                facingRight = false;
            }

            
        }

    }
}
