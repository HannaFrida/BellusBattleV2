using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlashBullet : Projectile
{
    // Start is called before the first frame update
    [SerializeField] private float destroyDelay = 2;
    [SerializeField] private float DetectingDistance = 0.1f;
	CameraFocus cf;

	private Rigidbody rb;
    private bool stopped;

    void Start()
    {

		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
		transform.position = new Vector3(transform.position.x, 0, transform.position.y);

        if(GetComponent<Rigidbody>() != null){
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, DetectingDistance)){
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.y);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
        

    }
	void OnCollisionEnter(Collision other)
	{

		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player")) // && Shooter != playerGo)
		{
			Debug.Log("Obstacle");
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			//GameObject MuzzleFlashIns = Instantiate(colliderPlayerVFX, pos, rot);
			//Destroy(MuzzleFlashIns, 3f);
			playerGo.GetComponent<PlayerHealth>().TakeDamage(damage);
			Debug.Log("Hit player");

			if (playerGo.GetComponent<PlayerHealth>().Health <= 0)
			{
				//playerGo.SetActive(false);
				//playerGo.GetComponent<PlayerHealth>().KillPlayer();
				cf.RemoveTarget(playerGo.transform);
			}
			/*
			PlayerDeathEvent pde = new PlayerDeathEvent{
				PlayerGo = other.gameObject,
				Kille = other.name,
				KilledBy = "No Idea-chan",
				KilledWith = "Bullets",
			};
			pde.FireEvent();
			*/
			//Die();
		}
		else if (playerGo.CompareTag("AI"))
		{
			playerGo.GetComponent<AI>().KillAI();
		}

		if (other.gameObject.tag == "Obstacle")
		{
			/*Debug.Log("Obstacle");
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			GameObject MuzzleFlashIns = Instantiate(colliderWallVFX, pos, rot);
			Destroy(MuzzleFlashIns, 3f);

			//GameObject MuzzleFlashIns = Instantiate(collideVFX, gameObject.transform.position, transform.rotation);
			//MuzzleFlashIns.transform.Rotate(Vector3.left * 90);
			//Destroy(gameObject);
			return;*/
		}

		if (other.gameObject.CompareTag("Target"))
		{
			GetComponent<Destroy>().gone();

		}

		if (other.gameObject.CompareTag("Breakable"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}

	}





	IEnumerator SlowDown()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            yield return new WaitForSeconds(0.1f);
        }
        stopped = true;
    }
}
