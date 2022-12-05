using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationXnade : MonoBehaviour
{
    public bool freez = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!freez) {
            gameObject.transform.Rotate(Vector3.forward * 1f);
        }
        

    }
    public void TimeToFreez()
    {
        freez = true;
    }
}
