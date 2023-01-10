using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Khaled Alraas
*/
public class PCamera : MonoBehaviour
{
    [SerializeField] private float startingAfter;
    private bool start = false;
    [SerializeField] private float speed;
    [SerializeField] private float platformSpawnTimer;
     private Vector3 bounds;
     private float t = 0;
     private float t2 = 0;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject platform;


    void Start()
    {
        bounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, GetComponent<Camera>().transform.position.z));
    }

    void LateUpdate()
    {
        if (t2 < startingAfter) t2 += Time.deltaTime;
        else start = true;

        if (start)
        {
            if (t < platformSpawnTimer) t += Time.deltaTime;
            else
            {
                GameObject g = Instantiate(platform);
                g.transform.position += new Vector3(Random.Range(bounds.x, bounds.x * -1), transform.position.y - 15, 0);
                t = 0;
            }
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
}
