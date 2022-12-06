using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMe2 : MonoBehaviour
{
    Aim da;
    bool tmp = true;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(da == null)
        {
            da = gameObject.GetComponent<Aim>();
            return;
        }
        if(!da.IsFacingRight())
        {
            transform.rotation *= Quaternion.AngleAxis(0, Vector3.right);

        }
        else if(da.IsFacingRight())
        {
            transform.rotation *= Quaternion.AngleAxis(180, Vector3.right);
        }
    }
}
