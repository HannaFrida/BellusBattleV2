
using System.Collections;
using UnityEngine;
using Random = System.Random;
public class Bullet : Projectile
{
	[SerializeField]
	[Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 5.0f;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	public AudioSource[] hitSounds;
	[SerializeField] private GameObject colliderWallVFX;
	[SerializeField] private GameObject colliderPlayerVFX;
	[SerializeField] private bool lighting;
	private float killMySelfTime = 0.4f;
	private float timer;
	bool canKillMyself;
	//public float bulletDamage;

	private void Start()
	{
		StartCoroutine(Shoot(lifeSpan));
	}

    private void Update()
    {
		timer += Time.deltaTime;
		if(timer >= killMySelfTime)
        {
			canKillMyself = true;
        }
    }
    void OnCollisionEnter(Collision other)
	{
		

		GameObject playerGo = other.gameObject;
        if (!canKillMyself)
        {
			Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), playerGo.GetComponent<Collider>(), true);
		}
		
		if (playerGo.CompareTag("Player")) // && Shooter != playerGo)
		{
            if (playerGo.GetComponent<PlayerDetails>().playerID == shooterID && canKillMyself == false) return;

			if (lighting)
			{
				playerGo.GetComponent<PlayerHealth>().PlayLighting();
			}
			
			ContactPoint contact = other.contacts[0];
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			if (colliderPlayerVFX != null)
			{
                GameObject MuzzleFlashIns = Instantiate(colliderPlayerVFX, pos, rot);
				//MuzzleFlashIns.transform.Rotate(Vector3.forward * -90);
				//MuzzleFlashIns.transform.Rotate(Vector3.right * 90);
				//MuzzleFlashIns.transform.Rotate(Vector3.up * );

				Destroy(MuzzleFlashIns, 3f);
            }
			PlayerHealth ph = playerGo.GetComponent<PlayerHealth>();
			
			if(ph.IsAlive == true && damage >= 1)
            {
				GameDataTracker.Instance.NewKillEvent(shooterID, playerGo.GetComponent<PlayerDetails>().playerID, weaponName, GameManager.Instance.RoundDuration);
			}
			ph.TakeDamage(damage);
			
			
			
			Die();
		}
		else if (playerGo.CompareTag("AI"))
		{
			playerGo.GetComponent<AI>().KillAI();
			Destroy(gameObject);
		}

		if (other.gameObject.tag.Equals("Obstacle"))
		{
			//Debug.Log("Obstacle");
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
		if (playerGo.CompareTag("Player") && shooter != playerGo)
		{
			PlayerHealth ph = playerGo.GetComponent<PlayerHealth>();
			Debug.Log("hehe");
            if (ph.IsAlive == true && damage >= 1)
            {
                Debug.Log("HUHDA");
                GameDataTracker.Instance.NewKillEvent(shooterID, playerGo.GetComponent<PlayerDetails>().playerID, weaponName, GameManager.Instance.RoundDuration);
            }
			ph.TakeDamage(damage);

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

		if (other.gameObject.tag.Equals("Obstacle"))
		{
			//Debug.Log("Obstacle");
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