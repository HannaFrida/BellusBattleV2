using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grenade : Projectile
{
	[SerializeField] [Tooltip("Time until the grenade explodes.")] 
	private float fuse = 5.0f;
	[SerializeField] [Tooltip("Size of the explosion.")]
	private float explosionSize = 5.0f;
	private CameraFocus cf; //shitfx
	[SerializeField] private GameObject objectToBoom;
	[SerializeField] private AudioSource explosionSound;
	[SerializeField] private GameObject bombMesh;

	private void Start(){
		cf = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>(); //shitfix
		StartCoroutine(StartFuse());
	}

	private IEnumerator StartFuse(){
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

	private void Explode()
	{
        if (bombMesh != null)
        {
			bombMesh.SetActive(false);
			explosionSound.Play();
			GameObject spawnVfx = Instantiate(objectToBoom, transform);
		}
		

		Collider[] hits = Physics.OverlapSphere(transform.position, explosionSize);
		for (int i = 0; i < hits.Length; i++){
			if (hits[i].CompareTag("Player"))
			{
				PlayerHealth ph = hits[i].GetComponent<PlayerHealth>();
				ph.TakeDamage(damage);
				cf.RemoveTarget(hits[i].transform); //shitfix
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

		
		// Delay before destroy
		Destroy(gameObject, 1f);
		//Die();
	}

	private void Die(){
		ExplodeEvent ee = new ExplodeEvent{
			Description = "Grenade " + name + " exploded!",
			ExplosionGo = gameObject
		};
		ee.FireEvent();
		
		Destroy(gameObject);
	}
}
