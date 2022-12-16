using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    private List<GameObject> currenthats;
    int random;
    private void Start()
    {
        currenthats = new List<GameObject>(hats);
    }

    // Start is called before the first frame update
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            GameObject place = other.gameObject.transform.FindChild("PutHatHere").gameObject;
            if (place.transform.childCount != 0)
            {
                currenthats.Add(currenthats[random]);
                Destroy(place.transform.GetChild(0).gameObject);
                // add
            }
            random = Random.Range(0, currenthats.Count);
            GameObject hat = Instantiate(currenthats[random], place.transform);
            if(place != null)
            {
                hat.transform.parent = place.transform;
                currenthats.Remove(currenthats[random]);
            }
            // object.transform.GetChild(Random.Range(0, hats.Count));
            //change something if needed
        }
    }
    // list of hats prefabs
    // go in a box collider
    // ge random hat
    //check boxes ...
    //ears
    // eyes
    // nose
}
