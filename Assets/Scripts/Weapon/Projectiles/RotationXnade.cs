using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationXnade : MonoBehaviour
{
    public bool freeze = false;
  
    // Update is called once per frame
    void Update()
    {
        if (!freeze) {
            Vector3 rotate = new Vector3(0f,0f,1f);
            gameObject.transform.Rotate(rotate);
        }
    }
    public void TimeToFreeze()
    {
        freeze = true;
    }
}
