using System.Collections;
using UnityEngine;

public class ExplodeListener : MonoBehaviour
{
	[SerializeField] private GameObject particleSys;

	private void Start()
	{
		ExplodeEvent.RegisterListener(OnExplosion);
	}

	private void OnExplosion(ExplodeEvent ee)
	{
		GameObject go = Instantiate(particleSys, ee.ExplosionGo.transform);
		go.transform.parent = null;
		ParticleSystem particle = go.GetComponent<ParticleSystem>();
		particle.Play();
		StartCoroutine(Explode(particle.main.duration, go));
	}
	
	private IEnumerator Explode(float seconds, Object go)
	{
		yield return new WaitForSeconds(seconds);

		Destroy(go);
	}

	private void OnDestroy()
	{
		ExplodeEvent.UnregisterListener(OnExplosion);
	}
}