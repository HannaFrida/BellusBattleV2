using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
/*
* Author Khaled Alraas
*/
public class Customization : MonoBehaviour, IDataPersistenceManagerHats
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
        DataPersistenceManager.Instance.LoadHatsData();
        availableHats = new List<GameObject>(hats);
    }
    private GameObject ChooseHatList()
    {
        if(hatList == HatList.FromList) return Instantiate(availableHats[random], place.transform);
        return availableHats[random];
    }
    private void RemoveHatList(GameObject placeObject)
    {
        if (hatList == HatList.FromList) Destroy(placeObject);
        else placeObject.transform.parent = gameObject.transform.GetChild(0);
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
                RemoveHatList(placeObject);
                return;
            }
        }
    }
    private void PutHat()
    {
        random = Random.Range(0, availableHats.Count - 1);
        GameObject hat = ChooseHatList();
        hat.transform.parent = place.transform;
        hat.transform.localPosition = Vector3.zero+positionAdjustment;
        hat.transform.localRotation = Quaternion.Euler(0, 0, 0);
        hat.transform.localScale = new Vector3(1, 1, 1);
        removedHats.Add(hat);
        availableHats.Remove(availableHats[random]);
        DataPersistenceManager.Instance.SaveHatsData();
    }
    private void OnApplicationQuit()
    {
        availableHats = hats;
        removedHats.Clear();
        DataPersistenceManager.Instance.SaveHatsData();
    }
    public void LoadData(HatsData data)
    {
        this.availableHats = data.availableHats;
        this.removedHats = data.removedHats;
    }

    public void SaveData(ref HatsData data)
    {
        data.availableHats = this.availableHats;
        data.removedHats = this.removedHats;
    }
    //check boxes ...
    //ears
    // eyes
    // nose
}