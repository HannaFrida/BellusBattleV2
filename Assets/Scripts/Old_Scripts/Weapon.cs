using UnityEngine;
using Random = System.Random;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	public Aim aim; // test to make bullet shoot in correct direction
	[SerializeField] [Tooltip("How accurate the firearm is.")] [Range(0, 1)]
    private float inaccuracy = 1.0f;
    [SerializeField] [Tooltip("Rounds per second.")]
    private float fireRate = 5.0f;
    [SerializeField] [Tooltip("Ammunition of the firearm.")]
    public int ammo = 6;
    [SerializeField] [Tooltip("What projectile is being fired.")] 
    private GameObject projectile;
    [SerializeField] [Tooltip("Where the projectile is fired from.")] 
    private GameObject projectileOrigin;
    [SerializeField] [Tooltip("The amount of force placed on the projectile.")]
    private float projectileForce = 100.0f;
    [SerializeField]
    [Tooltip("Muzzle flash")]
    public GameObject MuzzleFlash;

    public AudioSource shootSound;

    private float _nextTimeToFire;
    private bool _isFiring;
    private ParticleSystem _muzzleFlash;
    private Projectile _projectile;
    private readonly Random _random = new();

    private PlayerMovement player;

    //[SerializeField] GameObject revolver;
    //[SerializeField] GameObject Grenade;
    [SerializeField] GameObject Sword;
    [SerializeField] PickUp_ProtoV1 pickUp_Proto;

    private void Start(){
        _muzzleFlash = projectileOrigin.GetComponent<ParticleSystem>();
        _projectile = projectile.GetComponent<Projectile>();
    }

    public void OnPickUpWeapon()
    {
        player = gameObject.GetComponent<PlayerMovement>();
        aim = player.GetComponentInChildren<Aim>();
    }

    public void Fire(){
        MeshRenderer g = gameObject.GetComponent<MeshRenderer>();
        if (!g.enabled)
        {
            //Debug.Log(g.ToString());
            return;
        }
        if (ammo <= 0)
        {
            // "ta bort" vapnet från spelaren
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Weapon>().enabled = false;

            // Man ska alltid få tillbaka svärdet när bullet är slut
            Sword.GetComponentInChildren<MeshRenderer>().enabled = true;
            Sword.GetComponent<Sword_Prototype>().enabled = true;

            pickUp_Proto.isHoldingWeapon = false;
        }
        if (Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1.0f / fireRate;
            ammo--;
            if (_muzzleFlash != null){_muzzleFlash.Play();}

            GameObject firedProjectile = Instantiate(projectile, projectileOrigin.transform.position, transform.rotation);
            GameObject MuzzleFlashIns = Instantiate(MuzzleFlash, projectileOrigin.transform.position, transform.rotation);
            MuzzleFlashIns.transform.Rotate(Vector3.up * 90);
            shootSound.Play();

            //Force calculation uses 'aim.transform' could possibly use 'transform.localPosition' or 'transform.localRotation' instead.
            //Calculation is inefficient, could possibly be improved to simulate inaccuracy better.
            //Vector3 force = new Vector3(projectileForce * aim.transform.right.x * _random.Next(0, (int)inaccuracy * 100) / 100, projectileForce * aim.transform.right.y * _random.Next(0, (int)inaccuracy * 100) / 100, 0);
            Vector3 force = new Vector3(projectileForce * aim.transform.right.x, projectileForce * aim.transform.right.y, 0f);
            _projectile = firedProjectile.GetComponent<Projectile>();
            _projectile.GetComponent<Rigidbody>().AddForce(force);
            _projectile.SetShooter(gameObject);
        }
    }
}
