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
    private float railGunWaitForGone = 1.48f;

    /// <summary>
    /// Gets the ID of the one who is currently holding the weapon
    /// </summary>
    public int OwnerID { get => ownerID; }


    private void Start()
    {
        // Reload it
        gunsAmmo = weaponData.Ammo;

        projectile = weaponData.projectile;
        if (weaponData.projectile != null)
        {
            _projectile = projectile.GetComponent<Projectile>();
        }

        //dropTimer = 0f;
        //deSpawnTimer = 0f;
        //Drop();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (isDropped)
        {
            Despawn();
        }
    }

    private void Update()
    {
        // USED FOR FIRERATE
        timeSinceLastShot += Time.deltaTime;
        if (Time.deltaTime >= _nextTimeToFire)
        {
            // Might need to change calculation
            _nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);
            // = timeSinceLastShot + (1f / weaponData.fireRate);
        }

        
        // USED FOR DROP
        if (isStartTimerForDrop)
        {
            dropTimer += Time.deltaTime;
            //Debug.Log("droppper: " + dropTimer + " poda " + timeToWaitForPickup);
            if (dropTimer >= timeToWaitForPickup)
            {
                dropTimer = 0;
                isStartTimerForDrop = false;
                gameObject.GetComponent<BoxCollider>().enabled = true;

            }
        }
        if (railGoneTime)
        {
            railGoneTimer += Time.deltaTime;
            //Debug.Log("droppper: " + dropTimer + " poda " + timeToWaitForPickup);
            if (railGoneTimer >= railGunWaitForGone)
            {
                dropTimer = 0;
                railGoneTime = false;
                Drop();

            }
        }

        
        // USED FOR DE-SPAWNING
        if (isStartTimerForDeSpawn)
        {
            deSpawnTimer += Time.deltaTime;
            //Debug.Log("Despawn: " + deSpawnTimer);
            // No ammo && Time runs out
            if (deSpawnTimer >= timeToWaitForDeSpawn && gunsAmmo == 0)
            {

                deSpawnTimer = 0f;
                isStartTimerForDeSpawn = false;
                Despawn();
            }
        }

        if (gunsAmmo == 0 && weaponData.name != "RailGun")
        {
            Drop();
            Despawn();
        }

        if (gunsAmmo == 0 && weaponData.name != "Grenade" && weaponData.name != "GwynBolt" && weaponData.name != "RailGun")
        {
            Drop();
            Despawn();
        }

        // SPECIAL CASES

        if (gunsAmmo == 0 && weaponData.name == "Grenade")
        {
            Drop();
            Despawn();
        }
        if (gunsAmmo == 0 && weaponData.name == "GwynBolt")
        {
            Drop();
            Despawn();
        }
        if (gunsAmmo == 0 && weaponData.name == "Xnade")
        {
            Drop();
            Despawn();
        }
        
        if (gunsAmmo == 0 && weaponData.name == "RailGun")
        {
            railGoneTime = true;
        }


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
        if (other.gameObject.tag == "Player" && !isPickedUp)
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
        ownerAim = other.gameObject.GetComponentsInChildren<Aim>();

        
        //ownerAim = other.gameObject.GetComponentInChildren<Aim>();

        weaponManager = other.gameObject.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            if (weaponManager.EquippedWeapon == null)
            {
                playerShoot.shootInput += Shoot;
                playerShoot.dropInput += Drop;

                weaponManager.EquipWeapon(weaponData, gameObject);

                isStartTimerForDrop = false;
                isStartTimerForDeSpawn = false;
                deSpawnTimer = 0f;
                dropTimer = 0f;

                isPickedUp = true;


                //gunsAmmo = weaponData.Ammo;
            }

        }
    }

    private IEnumerator DeactivateAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    private void Despawn()
    {
        //Drop();

        
        Debug.Log("borde vara här");
        if (weaponData.name == "Xnade")
        {
            StartCoroutine(DeactivateAfterTime(2f));
        }
        if (weaponData.name == "GwynBolt")
        {
            VisualEffect bolt = GetComponentInChildren<VisualEffect>();
            bolt.enabled = false;
            StartCoroutine(DeactivateAfterTime(2f));
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Gun>().enabled = false;

       /* 
        if (gameObject.GetComponent<MeshFilter>().mesh != null)
        {
            Debug.Log("borde inte vara här");
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            GameObject despawnVFX = Instantiate(weaponData.DespawnVFX, transform.position, transform.rotation);
            despawnVFX.GetComponent<Despawn>().SetMesh(mesh);
        }
        */
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

        /*

        // Basic sword special case

        if (weaponData.name == "BasicSword" && timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && isPickedUp)
        {
            BasicSwordBehaviour bsb = swordMesh.GetComponent<BasicSwordBehaviour>();
            bsb.isAttacking = true;

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

            // Animation
            swordMesh.GetComponent<Animator>().SetBool("Attack", true);
            Debug.Log("Swosh");


        }

        */

        if (CanShoot())
        {
            gunsAmmo--;
            //Debug.Log(gunsAmmo);
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
            //if (weaponData.MuzzleFlash != null) { weaponData.MuzzleFlash.Play(); }
            if (weaponData.MuzzleFlashGameObject != null)
            {
                //Debug.Log("YOOOO");
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
                    _projectile.GetComponent<Rigidbody>().AddForce(dir += force);

                    timeSinceLastShot = 0;
                }

            }
            else if (weaponData.name == "RailGun"){
                GameObject firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

                _projectile = firedProjectile.GetComponent<Projectile>();
                _projectile.SetDamage(weaponData.damage);
                //_projectile.GetComponent<Rigidbody>().AddForce(force);
                _projectile.gameObject.transform.parent = muzzle.transform;


                // Lock aim
                StartCoroutine("DisableAimScript");

                timeSinceLastShot = 0;
            }
            else
            {
                firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation);

                _projectile = firedProjectile.GetComponent<Projectile>();
                _projectile.SetDamage(weaponData.damage);
                _projectile.GetComponent<Rigidbody>().AddForce(force);

                timeSinceLastShot = 0;
            }
            Debug.Log("ownerID : " + ownerID);
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


            //isStartTimerForDeSpawn = true;
        }

        gameObject.transform.SetParent(null);
        isDropped = true;
        // Otherwise it stays in DontDestroyOnLoad
        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        //gameObject.GetComponent<Gun>().enabled = false;
        //gameObject.transform.position = new Vector2(999999, 999999);
        //gameObject.SetActive(false);
        //ExecuteAfterTime(1f);
        //Debug.Log("fuck");
        //gameObject.GetComponent<BoxCollider>().enabled = true;


        // So that the previous owner can't shoot this gun
        playerShoot.shootInput = null;
        playerShoot.dropInput = null;
    }
    /*
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    */

    IEnumerator DisableAimScript()
    {

        foreach (Aim aim in ownerAim)
        {
            aim.enabled = false;
        }

        yield return new WaitForSeconds(1.6f);

        foreach (Aim aim in ownerAim)
        {
            aim.enabled = true;
        }
    }

}
