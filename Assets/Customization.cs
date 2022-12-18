using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Customization : MonoBehaviour
{
    enum HatList {FromList, FromScene};
    [SerializeField] private HatList hatList;
    [SerializeField] private List<GameObject> hats = new List<GameObject>();
    [SerializeField] private Vector3 positionAdjustment;
    private List<GameObject> availableHats;
    private List<GameObject> removedHats = new List<GameObject>();
    int random;
    GameObject place;
    private void Start()
    {
        availableHats = new List<GameObject>(hats);
    }
    private GameObject ChooseHatList()
    {
        if(hatList == HatList.FromList) return Instantiate(availableHats[random], place.transform);
        return availableHats[random];
    }

    // Start is called before the first frame update
    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            place = other.gameObject.GetComponentInChildren<CharacterCustimization>().transform.FindChild("Headslot").gameObject;
            EntryProtocol(place);
            PutHat();
            // object.transform.GetChild(Random.Range(0, hats.Count));
            //change something if needed
        }
    }

    private void EntryProtocol(GameObject placeObject)
    {
        //check if player have hat
        //if so remove the hat else continue
        placeObject = place.transform.GetChild(place.transform.childCount - 1).gameObject;
        foreach (GameObject hat in removedHats)
        {
            if (placeObject == hat)
            {
                availableHats.Add(hat);
                removedHats.Remove(hat);
                Destroy(placeObject);
                return;
            }
        }
    }
    private void PutHat()
    {
        random = Random.Range(0, availableHats.Count - 1);
        GameObject hat = ChooseHatList();
        hat.transform.position = place.transform.position + positionAdjustment;
        hat.transform.parent = place.transform;
        removedHats.Add(hat);
        availableHats.Remove(availableHats[random]);
    }
    //check boxes ...
    //ears
    // eyes
    // nose
}
