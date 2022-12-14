using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

/// <summary>
/// Put on every weapon
/// </summary>
public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponData weaponData; // The data of the weapon
    [SerializeField] private PlayerShoot playerShoot; // Actions
    [SerializeField] private int ownerID; // Player ID
    [SerializeField] private Aim[] ownerAim;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponSpawnerManager weaponSpawnerManager;

    [Tooltip("What projectile is being fired.")]
    private GameObject projectile;
    private Projectile _projectile;
    [Tooltip("Used for FireRate")]
    private float timeSinceLastShot;
    private float _nextTimeToFire;

    [Header("Sounds")]
    [SerializeField, Tooltip("Sound made when weapon out of ammo")]
    public AudioSource emptyGunSound;
    [SerializeField, Tooltip("Sound made when weapon shoots")]
    public AudioSource shootSound;

    [Header("Info")]
    [SerializeField] bool isPickedUp;
    [SerializeField] int gunsAmmo;

    [Header("Dropping")]
    [Tooltip("time before the weapon can be picked up again")]
    [SerializeField] float timeToWaitForPickup = 0.5f;
    bool isStartTimerForDrop;
    float dropTimer;
    bool isDropped;

    [Header("DeSpawning")]
    [Tooltip("time before the weapon can be picked up again")]
    [SerializeField] float timeToWaitForDeSpawn = 0.1f;
    bool isStartTimerForDeSpawn;
    float deSpawnTimer;

    [Header("Special cases")]
    [SerializeField] GameObject swordMesh;
    [SerializeField] bool BulletFollow = false;
    private GameObject firedProjectile;
    private bool railGoneTime;
    private float railGoneTimer = 0;
    private float railGunWaitForGone = 1.6f;

    /// <summary>
    /// Gets the ID of the one who is currently holding the weapon
    /// </summary>
    public int OwnerID { get => ownerID; }


    private void Start()
    {
        Debug.Log("fuck you");
        // Reload it
        gunsAmmo = weaponData.Ammo;

        projectile = weaponData.projectile;
        if (weaponData.projectile != null)
        {
            _projectile = projectile.GetComponent<Projectile>();
        }
        weaponSpawnerManager = FindObjectOfType<WeaponSpawnerManager>().GetComponent<WeaponSpawnerManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        weaponSpawnerManager = FindObjectOfType<WeaponSpawnerManager>().GetComponent<WeaponSpawnerManager>();
        // Otherwise weapon stays in DontDestroyOnLoad
        if (weaponSpawnerManager.GetTrashBin != null)
        {
            
            Despawn();
            Drop();
            Debug.Log("end trash");
        }
        
            


    }

    private void Update()
    {
        // USED FOR FIRERATE
        timeSinceLastShot += Time.deltaTime;
        if (Time.deltaTime >= _nextTimeToFire)
        {
            _nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);
        }


        // USED FOR DROP
        if (isStartTimerForDrop)
        {
            dropTimer += Time.deltaTime;
            if (dropTimer >= timeToWaitForPickup)
            {
                dropTimer = 0;
                isStartTimerForDrop = false;
                gameObject.GetComponent<BoxCollider>().enabled = true;

            }
        }
        
        /*
        // USED FOR DE-SPAWNING
        if (isStartTimerForDeSpawn)
        {
            deSpawnTimer += Time.deltaTime;
            if (deSpawnTimer >= timeToWaitForDeSpawn && gunsAmmo == 0)
            {

                deSpawnTimer = 0f;
                isStartTimerForDeSpawn = false;
                Despawn();
            }
        }
        */

        if (gunsAmmo == 0 && weaponData.name != "RailGun")
        {
            Drop();
            if (shootSound.isPlaying)
            {
                return;
            }
            Despawn();
            
        }

        // SPECIAL CASES 
        if (gunsAmmo == 0 && weaponData.name == "RailGun")
        {
            railGoneTime = true;
        }

        if (railGoneTime)
        {
            railGoneTimer += Time.deltaTime;
            if (railGoneTimer >= railGunWaitForGone)
            {
                dropTimer = 0;
                railGoneTime = false;

                Despawn();
                foreach (Aim aim in ownerAim)
                {
                    aim.enabled = true;
                }
                Drop();
                if (shootSound.isPlaying)
                {
                    return;
                }
                gameObject.SetActive(false);
            }
        }

        if (weaponManager != null)
        {
            //weaponManager.OnlyOneChild();
            if (weaponManager.weaponSlot.childCount > 0 && weaponManager.EquippedWeapon == null && !isPickedUp)
            {
                playerShoot.shootInput += Shoot;
                playerShoot.dropInput += Drop;

                GameObject currentWeapon = weaponManager.weaponSlot.GetChild(0).gameObject;
                Debug.Log(currentWeapon.name);
                weaponManager.EquipWeapon(currentWeapon.GetComponent<Gun>().weaponData, currentWeapon);

                Debug.Log("shitty fix fryhrfyuftu");
                isStartTimerForDrop = false;
                isStartTimerForDeSpawn = false;
                deSpawnTimer = 0f;
                dropTimer = 0f;

                isPickedUp = true;
            }
        }


            /*
            // For making last shot of gun heard
            if (weaponSpawnerManager.GetTrashBin != null)
            {
                if (weaponSpawnerManager.GetTrashBin.childCount > 0)
                {
                    for (int i = 0; i < weaponSpawnerManager.GetTrashBin.childCount; i++)
                    {
                        Debug.Log(i);
                        weaponSpawnerManager.GetTrashBin.transform.GetChild(i).gameObject.SetActive(true);
                        weaponSpawnerManager.GetTrashBin.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;

                        //weaponSpawnerManager.GetTrashBin.transform.GetChild(i).GetComponent<MeshFilter>().gameObject.SetActive(false);
                    }
                }
            }
            */

            /*
            if (BulletFollow && firedProjectile != null)
            {
                Debug.Log("RAIL");
                firedProjectile.transform.position = muzzle.transform.position;
                firedProjectile.transform.rotation = transform.rotation;
            }
            */


        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !isPickedUp)
        {
            PickUp(other);
        }
    }

    public void PickUp(Collider other)
    {
        playerShoot = other.gameObject.GetComponent<PlayerShoot>();

        // Check who the owner of the weapon is 
        ownerID = other.gameObject.GetComponent<PlayerDetails>().playerID;

        // Get the reference for the players aim
        ownerAim = other.gameObject.GetComponentsInParent<Aim>();

        weaponManager = other.gameObject.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            /*
            if (weaponManager.weaponSlot.childCount > 0)
            {
                return;
            }
            */
            //weaponManager.OnlyOneChild();
            if (weaponManager.weaponSlot.childCount > 0 && weaponManager.EquippedWeapon == null)
            {
                playerShoot.shootInput += Shoot;
                playerShoot.dropInput += Drop;

                GameObject currentWeapon = weaponManager.weaponSlot.GetChild(0).gameObject;
                weaponManager.EquipWeapon(currentWeapon.GetComponent<Gun>().weaponData, currentWeapon);

                Debug.Log("shitty fix");
                isStartTimerForDrop = false;
                isStartTimerForDeSpawn = false;
                deSpawnTimer = 0f;
                dropTimer = 0f;

                isPickedUp = true;
            }
            
            if (weaponManager.EquippedWeapon == null)
            {
                playerShoot.shootInput += Shoot;
                playerShoot.dropInput += Drop;

                weaponManager.EquipWeapon(weaponData, gameObject);
                Debug.Log("fuck igndfd");
                isStartTimerForDrop = false;
                isStartTimerForDeSpawn = false;
                deSpawnTimer = 0f;
                dropTimer = 0f;

                isPickedUp = true;
            }
        }
    }

    private IEnumerator DeactivateAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        //Drop();
        gameObject.SetActive(false);
    }

    private void Despawn()
    {
        if (weaponData.name == "GwynBolt")
        {
            VisualEffect bolt = GetComponentInChildren<VisualEffect>();
            bolt.enabled = false;
            StartCoroutine(DeactivateAfterTime(2f));
        }
        // Because Grenade has 2 meshfilters
        else if (weaponData.name == "Grenade")
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

            foreach (MeshFilter mesh in meshFilters)
            {

                mesh.gameObject.SetActive(false);
            }
        }

        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        meshFilter.gameObject.SetActive(false);

        if (weaponData.name == "Xnade")
        {
            StartCoroutine(DeactivateAfterTime(5f));
        }
        else
        { 
            gameObject.SetActive(false);
        }
        

        gameObject.GetComponent<BoxCollider>().enabled = false;
        //gameObject.GetComponent<Gun>().enabled = false;


        if (gameObject.GetComponent<MeshFilter>() != null)
        {
            Debug.Log("borde inte vara h?r");
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            GameObject despawnVFX = Instantiate(weaponData.DespawnVFX, transform.position, transform.rotation);
            despawnVFX.GetComponent<Despawn>().SetMesh(mesh);
        }

    }

    private bool CanShoot() => timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && gunsAmmo > 0 && isPickedUp;//!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f); //weaponData.Ammo > 0

    private void Shoot()
    {
        /*
        if (gunsAmmo == 0 || weaponData.name != "BasicSword")
        {
             // Play click sound to indicate no ammo left
             if (emptyGunSound != null)
               {
                  emptyGunSound.Play();

              }
            Debug.Log("Click clack");
        }
        */


        if (CanShoot())
        {
            gunsAmmo--;

            if (shootSound != null)
            {
                shootSound.Play();
            }

            //Sound
            if (weaponData.shootAttackSound != null)
            {
                weaponData.shootAttackSound.Play();
            }

            //VFX
            if (weaponData.MuzzleFlashGameObject != null)
            {
                GameObject MuzzleFlashIns = Instantiate(weaponData.MuzzleFlashGameObject, muzzle.transform.position, transform.rotation);
                MuzzleFlashIns.transform.Rotate(Vector3.up * 90);
                Destroy(MuzzleFlashIns, 4f);
            }

            float forceForwrd = weaponData.projectileForce;
            float aimx = muzzle.transform.forward.x;
            float aimy = muzzle.transform.forward.y;
            Vector3 force = new Vector3(forceForwrd * aimx, forceForwrd * aimy, 0f);

            if (weaponData.usesSpread)
            {
                for (int i = 0; i < weaponData.pelletCount; i++)
                {
                    GameObject bullet = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

                    Vector3 dir = transform.forward + new Vector3(Random.Range(-weaponData.spreadFactor, weaponData.spreadFactor), Random.Range(-weaponData.spreadFactor, weaponData.spreadFactor), 0f);

                    _projectile = bullet.GetComponent<Projectile>();
                    _projectile.SetDamage(weaponData.damage);
                    _projectile.SetShooterID(ownerID);

                    _projectile.GetComponent<Rigidbody>().AddForce(dir += force);

                    timeSinceLastShot = 0;
                }

            }
            else if (weaponData.name == "RailGun")
            {
                GameObject firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

                _projectile = firedProjectile.GetComponent<Projectile>();
                _projectile.SetDamage(weaponData.damage);
                _projectile.gameObject.transform.parent = muzzle.transform;

                // Lock aim
                StartCoroutine("DisableAimScript");

                timeSinceLastShot = 0;
            }
            else
            {
                if (weaponData.name == "Xnade")
                {
                    firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, weaponData.projectile.transform.rotation);
                }
                else
                {
                    firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);
                }
                

                _projectile = firedProjectile.GetComponent<Projectile>();
                _projectile.SetDamage(weaponData.damage);
                _projectile.GetComponent<Rigidbody>().AddForce(force);

                timeSinceLastShot = 0;
            }
            _projectile.SetShooterID(ownerID);
            _projectile.SetWeaponName(weaponData.name);
        }
    }

    public void Drop()
    {
        isPickedUp = false;
        if (weaponManager != null)
        {
            weaponManager.UnEquipWeapon(gameObject);
        }

        isStartTimerForDrop = true;
        if (gunsAmmo == 0)
        {
            isStartTimerForDeSpawn = true;
        }

        isDropped = true;

        // So that the previous owner can't shoot this gun
        playerShoot.shootInput = null;
        playerShoot.dropInput = null;

        gameObject.transform.SetParent(null);
        // Otherwise it stays in DontDestroyOnLoad
        if (weaponSpawnerManager.GetTrashBin != null)
        {
            gameObject.transform.SetParent(weaponSpawnerManager.GetTrashBin);
            
        }
    }

    IEnumerator DisableAimScript()
    {
        yield return new WaitForSeconds(0.6f);
        foreach (Aim aim in ownerAim)
        {
            aim.enabled = false;
        }

        yield return new WaitForSeconds(1f);

        foreach (Aim aim in ownerAim)
        {
            aim.enabled = true;
        }
    }

}
