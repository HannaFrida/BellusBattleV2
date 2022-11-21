using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(CameraFocus))]
public class CameraStartFocus : MonoBehaviour
{
	[SerializeField] [Tooltip("All the targets to track at the start of the game.")]
	private GameObject[] startTargets;
	[SerializeField] [Tooltip("For how long in seconds the camera should wait until removing the zoom targets" + "\n0 to disable and to use scripts.")]
	private float waitTime = 5.0f;
	
	private CameraFocus _cf;

	private void Start()
	{
		_cf = GetComponent<CameraFocus>();
		if(startTargets.Length != 0) {
			for (int i = 0; i < startTargets.Length; i++)
			{
				_cf.AddTarget(startTargets[i].transform);
			}
			
			if (waitTime != 0)
			{
				StartCoroutine(Zoom(waitTime));
			}
		}
	}
	
	public void RemoveTargets()
	{
		for (int i = 0; i < startTargets.Length; i++)
		{
			_cf.RemoveTarget(startTargets[i].transform);
		}
	}

	private IEnumerator Zoom(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		RemoveTargets();
	}
}


