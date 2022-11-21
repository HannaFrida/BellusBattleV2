using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    float spawnTime = .1f;

    public WeaponData pool;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            //GameObject obj = pool.GetPooledObject(transform.position, transform.rotation);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
