using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Customization : MonoBehaviour
{
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    private List<GameObject> availableHats;
    private List<GameObject> removedHats = new List<GameObject>();
    int random;
    private void Start()
    {
        availableHats = new List<GameObject>(hats);
    }

    // Start is called before the first frame update
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject place = other.gameObject.GetComponentInChildren<CharacterCustimization>().transform.FindChild("Headslot").gameObject;
            EntryProtocol(place);
            PutHat(place);
            // object.transform.GetChild(Random.Range(0, hats.Count));
            //change something if needed
        }
    }

    private void EntryProtocol(GameObject place)
    {
        //check if player have hat
        //if so remove the hat else continue
        place = place.transform.GetChild(place.transform.childCount - 1).gameObject;
        foreach (GameObject hat in removedHats)
        {
            if (place == hat)
            {
                availableHats.Add(hat);
                removedHats.Remove(hat);
                Destroy(place);
                return;
            }
        }
    }
    private void PutHat(GameObject place)
    {
        random = Random.Range(0, availableHats.Count - 1);
        GameObject hat = Instantiate(availableHats[random], place.transform);
        hat.transform.parent = place.transform;
        removedHats.Add(hat);
        availableHats.Remove(availableHats[random]);
    }
    //check boxes ...
    //ears
    // eyes
    // nose
}
