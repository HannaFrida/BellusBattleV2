
using System.Collections;
using UnityEngine;
using Random = System.Random;
public class Bullet : Projectile
{
	[SerializeField]
	[Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;
	CameraFocus cf;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	public AudioSource[] hitSounds;
	[SerializeField] private GameObject colliderWallVFX;
	[SerializeField] private GameObject colliderPlayerVFX;

	//public float bulletDamage;

	private void Start()
	{
		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
		StartCoroutine(Shoot(lifeSpan));
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
			if (colliderPlayerVFX != null)
			{
                GameObject MuzzleFlashIns = Instantiate(colliderPlayerVFX, pos, rot);
                Destroy(MuzzleFlashIns, 3f);
            }
			
			playerGo.GetComponent<PlayerHealth>().TakeDamage(damage);
			Debug.Log("Hit player");

			Die();
		}
		else if (playerGo.CompareTag("AI"))
		{
			playerGo.GetComponent<AI>().KillAI();
		}

		if (other.gameObject.tag == "Obstacle")
		{
			Debug.Log("Obstacle");
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			if (colliderWallVFX != null)
			{
                GameObject MuzzleFlashIns = Instantiate(colliderWallVFX, pos, rot);
                Destroy(MuzzleFlashIns, 3f);
            }
			
			

			//GameObject MuzzleFlashIns = Instantiate(collideVFX, gameObject.transform.position, transform.rotation);
			//MuzzleFlashIns.transform.Rotate(Vector3.left * 90);
			Destroy(gameObject);
			return;
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

		if (hitSounds.Length > 0)
		{
			hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)].Play();
		}

	}
	private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player") && Shooter != playerGo)
		{
			playerGo.GetComponent<PlayerHealth>().TakeDamage(damage);

			if (playerGo.GetComponent<PlayerHealth>().Health <= 0)
			{
				//playerGo.SetActive(false);
				//playerGo.GetComponent<PlayerHealth>().KillPlayer();
				cf.RemoveTarget(playerGo.transform);
			}

			PlayerDeathEvent pde = new PlayerDeathEvent
			{
				PlayerGo = other.gameObject,
				Kille = other.name,
				KilledBy = "No Idea-chan",
				KilledWith = "Bullets",
			};
			pde.FireEvent();

			Die();
		}
		else if (playerGo.CompareTag("AI"))
		{
			playerGo.GetComponent<AI>().KillAI();
		}

		if (other.gameObject.CompareTag("Target"))
		{
			GetComponent<Destroy>().gone();

		}

		if (other.gameObject.tag == "Obstacle")
		{
			Debug.Log("Obstacle");
			//GameObject MuzzleFlashIns = Instantiate(collideVFX, other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position), other.transform.rotation);
			//MuzzleFlashIns.transform.Rotate(Vector3.left * 90);
			//Destroy(gameObject);
			//return;
		}

		if (other.gameObject.CompareTag("Breakable"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}

		if (hitSounds.Length > 0)
		{
			hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)].Play();
		}

	}

	/*
	public void SetDamage(float setTo)
    {
		bulletDamage = setTo;
    }
	*/

	private IEnumerator Shoot(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
}