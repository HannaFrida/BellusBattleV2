using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationXnade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.Rotate(Vector3.forward * 0.25f);

    }
}
