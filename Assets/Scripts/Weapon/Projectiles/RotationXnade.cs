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
            Vector3 rotate = new Vector3(0f,0f,1f);
            gameObject.transform.Rotate(rotate);
        }
        

    }
    public void TimeToFreez()
    {
        freez = true;
    }
}
