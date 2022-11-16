using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GroundSlashGun : MonoBehaviour
{
    /**
     * Put on every weapon
    **/
    [Header("References")]
    [SerializeField] private WeaponData weaponData; // The data of the weapon
    [SerializeField] private PlayerShoot playerShoot; // Actions
    [SerializeField] private int ownerID; // Player ID
    [SerializeField] private WeaponManager weaponManager;

    public int OwnerID { get => ownerID; }

    [Tooltip("What projectile is being fired.")]
    private GameObject projectile;

    private Projectile _projectile;
    [SerializeField] private Transform muzzle;
    [Tooltip("Used for FireRate")]
    private float timeSinceLastShot;
    private float _nextTimeToFire;

    [Header("Sounds")]
    [SerializeField, Tooltip("Sound made when weapon out of ammo")]
    //public AudioSource emptyGunSound;
    public AudioSource shootSound;

    [SerializeField] private PlayerMovement player;
    [SerializeField] public Aim aim; // test to make bullet shoot in correct direction

    [SerializeField] bool isPickedUp;

    [SerializeField] int gunsAmmo;
    [SerializeField] GameObject swordMesh;

    [Header("Dropping")]
    bool isStartTimerForDrop;
    [Tooltip("time before the weapon can be picked up again")]
    [SerializeField] float timeToWaitForPickup = 2f;
    float dropTimer;

    [Header("DeSpawning")]
    bool isStartTimerForDeSpawn;
    [Tooltip("time before the weapon can be picked up again")]
    [SerializeField] float timeToWaitForDeSpawn = 2f;
    float deSpawnTimer;

    private Vector3 destination;
    private GroundSlashBullet groundSlash;

    private void Start()
    {
        // Reload it
        gunsAmmo = weaponData.Ammo;

        projectile = weaponData.projectile;
        if (weaponData.projectile != null)
        {
            _projectile = projectile.GetComponent<Projectile>();
        }

        dropTimer = 0f;
        deSpawnTimer = 0f;
        //Drop();


    }

    private void Update()
    {
        // USED FOR FIRERATE
        timeSinceLastShot += Time.deltaTime;
        if (Time.deltaTime >= _nextTimeToFire)
        {
            // Might need to change calculation
            _nextTimeToFire = timeSinceLastShot / (weaponData.fireRate / 60f);
        }

        if (gunsAmmo == 0 && weaponData.name != "BasicSword")
        {
            // Placeholder för när vi fixat riktiga drop
            Drop();
        }

        /*
        // USED FOR DE-SPAWNING
        if (!isStartTimerForDeSpawn)
        {
            return;
        }
        deSpawnTimer += Time.deltaTime;
        //Debug.Log("Despawn: " + deSpawnTimer);
        if (deSpawnTimer >= timeToWaitForDeSpawn && gunsAmmo == 0) // No ammo && Time runs out
        {
            isStartTimerForDeSpawn = false;
            deSpawnTimer = 0f;
            // Delete this gun
            Drop();
            gameObject.SetActive(false);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
       */
        /*
        // USED FOR DROP
        if (!isStartTimerForDrop)
        {
            return;
        }
        dropTimer += Time.deltaTime;
        //Debug.Log("droppper: " + dropTimer +" poda " + timeToWaitForPickup);
        if (dropTimer >= timeToWaitForPickup)
        {
            dropTimer = 0;
            isStartTimerForDrop = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isPickedUp)
        {
            playerShoot = other.gameObject.GetComponent<PlayerShoot>();

            // Check who the owner of the weapon is 
            ownerID = other.gameObject.GetComponent<PlayerDetails>().playerID;

            weaponManager = other.gameObject.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                if (weaponManager.EquippedWeapon == null)
                {
                    playerShoot.shootInput += Shoot;
                    playerShoot.dropInput += Drop;
                    weaponManager.EquipWeapon(weaponData, gameObject);
                    deSpawnTimer = 0f;
                    isPickedUp = true;

                    //gunsAmmo = weaponData.Ammo;
                }

            }
        }
    }

    private bool CanShoot() => timeSinceLastShot > 1f / (weaponData.fireRate / 60f) && gunsAmmo > 0 && isPickedUp;//!gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f); //weaponData.Ammo > 0

    private void Shoot()
    {
        if (gunsAmmo == 0 || weaponData.name != "BasicSword")
        {
            // Play click sound to indicate no ammo left
            // if (emptyGunSound != null)
            //   {
            //      emptyGunSound.Play();

            //  }
            //Debug.Log("Click clack");
        }

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
                Debug.Log("YOOOO");
                GameObject MuzzleFlashIns = Instantiate(weaponData.MuzzleFlashGameObject, muzzle.transform.position, transform.rotation);
                MuzzleFlashIns.transform.Rotate(Vector3.up * 90);
                Destroy(MuzzleFlashIns, 4f);
            }


            float forceForwrd = weaponData.projectileForce;
            float aimx = muzzle.transform.forward.x;
            float aimy = muzzle.transform.forward.y;
            GameObject firedProjectile = Instantiate(weaponData.projectile, muzzle.transform.position, transform.rotation) as GameObject;
            groundSlash = firedProjectile.GetComponent<GroundSlashBullet>();
            firedProjectile.GetComponent<Rigidbody>().velocity = transform.forward * forceForwrd;
            firedProjectile.transform.Rotate(Vector3.right * 180);

            //emptyGunSound.Play();
            // mainly used for Lobby gun atm
            firedProjectile.GetComponent<Bullet>().SetDamage(weaponData.damage);

            
            
            Vector3 force = new Vector3(forceForwrd * aimx, forceForwrd * aimy , 0f);
            _projectile = firedProjectile.GetComponent<Projectile>();
            _projectile.SetDamage(weaponData.damage);
            //_projectile.GetComponent<Rigidbody>().AddForce(force);

            timeSinceLastShot = 0;
        }
    }
    

    public void Drop()
    {
        isPickedUp = false;
        if (weaponManager != null)
        {
            weaponManager.UnEquipWeapon(gameObject);
        }

        gameObject.transform.SetParent(null);
        // Otherwise it stays in DontDestroyOnLoad
        //SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        isStartTimerForDrop = true;
        isStartTimerForDeSpawn = true;
        //gameObject.GetComponent<Gun>().enabled = false;

        gameObject.transform.position = new Vector2(999999, 999999);




        gameObject.SetActive(false);

        //ExecuteAfterTime(1f);

        //Debug.Log("fuck");
        //gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

}
