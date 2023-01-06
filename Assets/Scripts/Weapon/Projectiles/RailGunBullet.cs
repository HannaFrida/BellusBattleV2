using System.Collections;
using UnityEngine;
using Random = System.Random;

public class RailGunBullet : Projectile
{
	[SerializeField]
	[Tooltip("For how long the bullet will exist for in seconds.")]
	private float lifeSpan = 0f;
	[SerializeField, Tooltip("Sound made when bullet hits something")]
	private AudioSource[] hitSounds;
	[SerializeField] private GameObject colliderWallVFX;
	[SerializeField] private GameObject colliderPlayerVFX;
	[SerializeField] private Collider col;
	[SerializeField] private Collider col2;
	[SerializeField] private float colOn = 0.015f;
	[SerializeField] private float Gone = 0.015f;
	[SerializeField] private bool xnade = false;
	[SerializeField] private float stopMove = 0.1f;
	private bool stopMovement = false;
	//public float bulletDamage;

	private void Start()
	{
		StartCoroutine(Shoot(lifeSpan));
	}
    private void Update()
    {
		if (xnade && stopMovement)
		{
			gameObject.GetComponent<Rigidbody>().freezeRotation = true;
			gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponentInChildren<RotationXnade>().TimeToFreeze();
		}
	}

    private void OnTriggerEnter(Collider other)
	{
		GameObject playerGo = other.gameObject;
		if (playerGo.CompareTag("Player"))
		{
            PlayerHealth ph = playerGo.GetComponent<PlayerHealth>();
			if(ph.IsAlive == true)
            {
				GameDataTracker.Instance.NewKillEvent(shooterID, playerGo.GetComponent<PlayerDetails>().playerID, weaponName, GameManager.Instance.RoundDuration);
			}

            ph.TakeDamage(damage);
			ph.PlayFire();
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
		yield return new WaitForSeconds(stopMove);
		Stop();
		yield return new WaitForSeconds(colOn);
		Killcol();
		yield return new WaitForSeconds(seconds);
		KillcolOff();
		yield return new WaitForSeconds(seconds);
		Die();
	}

	private void Die()
	{
		Destroy(gameObject);
	}
	private void Stop()
	{
		stopMovement = true;
	}
	private void KillcolOff()
	{
		col.enabled = false;
		if (col2 != null)
		{
			col2.enabled = false;
		}
		//col2.enabled = true;

	}
	private void Killcol()
	{
		col.enabled = true;
		if (col2 != null)
        {
			col2.enabled = true;
		}
		

	}
}

