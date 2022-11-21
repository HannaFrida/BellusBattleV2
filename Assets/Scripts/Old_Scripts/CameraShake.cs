using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	/*
	 * Shakes the camera given a duration of seconds and a magnitude of shaking.
	 */
	public IEnumerator Shake(float seconds, float magnitude)
	{
		Vector3 originalPosition = transform.localPosition;
		float elapsed = 0.0f;
		while (elapsed < seconds)
		{
			float x = Random.Range(-1.0f, 1.0f) * magnitude;
			float y = Random.Range(-1.0f, 1.0f) * magnitude;

			transform.localPosition = new Vector3(x, y, originalPosition.z);

			elapsed += Time.deltaTime;
			
			yield return null;
		}

		transform.localPosition = originalPosition;
	}
}
