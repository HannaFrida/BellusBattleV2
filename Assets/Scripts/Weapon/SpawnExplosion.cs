using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{

    float timer;
    [SerializeField] private float boomTime = 6f;
    [SerializeField] private GameObject objectToBoom;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > boomTime)
        {
            GameObject spawned = Instantiate(objectToBoom, transform);
            spawned.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
