using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSpawnerManager : MonoBehaviour
{
    [SerializeField] private float initialSpawnWeaponsTimer;
    [SerializeField] private float spawnWeaponsTimer;
    [SerializeField] private float numberOfWeaponsToSpawn;
    [SerializeField] private WeaponSpawner[] spawners;
    private List<WeaponSpawner> spawnersToChooseFrom = new List<WeaponSpawner>();
    private List<WeaponSpawner> choosenSpawners = new List<WeaponSpawner>();
    private bool isReadyToSpawn = false;
    [SerializeField] private Transform trashBin;

    public Transform GetTrashBin { get { return trashBin; } }

    void Start()
    {
        ControlNumberOfWeaponToSpawners();
        ÍnitialSpawnersToChooseFrom();
        StartCoroutine(InitialSpawnWeapons());
    }
    private void ControlNumberOfWeaponToSpawners()
    {

        if (numberOfWeaponsToSpawn > spawners.Length)
        {
            numberOfWeaponsToSpawn = spawners.Length;
            //Debug.LogError("\"numberOfWeaponsToSpawn\" is larger than the number of spawners that exists in the Scene");
        }
    }
    private void ÍnitialSpawnersToChooseFrom()
    {
        foreach (var spawner in spawners)
        {
            spawnersToChooseFrom.Add(spawner);
        }
    }
    IEnumerator InitialSpawnWeapons()
    {
        yield return new WaitForSeconds(initialSpawnWeaponsTimer);
        ChooseSpawnReapeatProtocol();
    }

    void Update()
    {
        if (isReadyToSpawn) StartCoroutine(SpawnWeapons());
    }
    IEnumerator SpawnWeapons()
    {
        yield return new WaitForSeconds(spawnWeaponsTimer);
        ChooseSpawnReapeatProtocol();
    }
    private void ChooseSpawnReapeatProtocol()
    {
        ChooseRandomSpawners();
        SpawnWeaponsInSpawners();
        choosenSpawners.Clear();
        StartCoroutine(SpawnWeapons());
    }
    private void ChooseRandomSpawners()
    {
        int temporaryNumber;
        for (int i = 0; i < numberOfWeaponsToSpawn && i < spawners.Length && spawnersToChooseFrom.Count > 0; i++)
        {
            temporaryNumber = Random.Range(0, spawnersToChooseFrom.Count);
            choosenSpawners.Add(spawnersToChooseFrom.ElementAt(temporaryNumber));
            //spawnersToChooseFrom.RemoveAt(temporaryNumber);
        }
        //Debug.Log(spawnersToChooseFrom.Count);
    }
    private void SpawnWeaponsInSpawners()
    {
        foreach (var spawner in choosenSpawners)
        {
            if(spawner.HasWeapons() == false)
            {
                spawner.SpawnRandomWeapon();
            }
            
        }
    }
    public void AddEmptySpawnerToChooseFrom(WeaponSpawner spawner)
    {
        spawnersToChooseFrom.Add(spawner);
    }

}
