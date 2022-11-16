using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private VisualEffect doubleJumpVFX;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDoubleJumpVFX(GameObject go)
    {
        Debug.Log(go);
        doubleJumpVFX.transform.position = go.transform.position;
        doubleJumpVFX.Play();
    }
   
}
