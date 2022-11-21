using System.Collections;
using UnityEngine;
using Random = System.Random;

public class RailGunBullet : Projectile
{
	[SerializeField]
	[Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 0.015f;
	CameraFocus cf;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	public AudioSource[] hitSounds;
	[SerializeField] private GameObject colliderWallVFX;
	[SerializeField] private GameObject colliderPlayerVFX;
	[SerializeField] private Collider col;
	[SerializeField] private float colOn = 0.015f;
	//public float bulletDamage;

	private void Start()
	{
		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
		StartCoroutine(Shoot(lifeSpan));
	}
	
	private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player"))
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

			//Die();
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
		yield return new WaitForSeconds(colOn);
		killcol();
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
	private void killcol()
	{
		col.enabled = true;

	}
}

