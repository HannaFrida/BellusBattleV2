using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grenade : Projectile
{
    [SerializeField]
    [Tooltip("Time until the grenade explodes.")]
    private float fuse = 5.0f;
    [SerializeField]
    [Tooltip("Size of the explosion.")]
    private float explosionSize = 5.0f;
    private CameraFocus cf; //shitfx
    [SerializeField] private GameObject objectToBoom;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] private GameObject bombMesh;
    [SerializeField] Collider[] hits;
    [SerializeField] bool lighting =false;
    [SerializeField] bool fire = false;

    private void Start()
    {
        //cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>(); //shitfix
        StartCoroutine(StartFuse());
    }

    private IEnumerator StartFuse()
    {
        yield return new WaitForSeconds(fuse);
        Explode();
    }

    public void OnPickUp(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            StartFuse();
        }
    }

    public void Explode()
    {
        Destroy(gameObject, 0.24f);
        if (bombMesh != null)
        {
            bombMesh.SetActive(false);
            explosionSound.Play();
            GameObject spawnVfx = Instantiate(objectToBoom, transform.position,transform.rotation);
            Destroy(spawnVfx, 0.3f);
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Rigidbody RB = GetComponent<Rigidbody>();
            RB.velocity = Vector3.zero;
            
        }

        hits = Physics.OverlapSphere(transform.position, explosionSize);
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].name);
            if (hits[i].CompareTag("Player"))
            {
                if(lighting)
                {
                    hits[i].GetComponent<PlayerHealth>().PlayLighting();
                }
                if (fire)
                {
                    hits[i].GetComponent<PlayerHealth>().PlayFire();
                }

                PlayerHealth ph = hits[i].GetComponent<PlayerHealth>();
                ph.TakeDamage(damage);
                Debug.Log("playerfound");
                //cf.RemoveTarget(hits[i].transform); //shitfix
                //pickUp_Proto.isHoldingWeapon = false;

                /*
				PlayerDeathEvent pde = new PlayerDeathEvent{
					PlayerGo = hits[i].gameObject,
					Kille = hits[i].name,
					KilledBy = "No Idea-chan",
					KilledWith = "Bullets",
				};
				pde.FireEvent();
				*/
            }
            if (hits[i].CompareTag("Door"))
            {
                hits[i].GetComponent<Door>().DestroyDoor();
            }
            if (hits[i].CompareTag("Breakable"))
            {
                Destroy(hits[i].gameObject);
            }
        }

        //hits = null;

        // Delay before destroy
        
        //Die();
    }


    private void Die()
    {
        ExplodeEvent ee = new ExplodeEvent
        {
            Description = "Grenade " + name + " exploded!",
            ExplosionGo = gameObject
        };
        ee.FireEvent();
        hits = null;
        Destroy(gameObject);
    }
}
