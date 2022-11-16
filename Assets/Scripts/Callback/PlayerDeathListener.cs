using UnityEngine;

public class PlayerDeathListener : MonoBehaviour
{
	private CameraFocus _cf;

	private void Start()
	{
		PlayerDeathEvent.RegisterListener(OnPlayerDeath);
		if(Camera.main != null) {
			_cf = Camera.main.GetComponent<CameraFocus>();
		}
	}

	private void OnPlayerDeath(PlayerDeathEvent pde)
	{
		Debug.Log("Player " + pde.Kille + " was killed by " + pde.KilledBy + " using " + pde.KilledWith + ".");
		_cf.RemoveTarget(pde.PlayerGo.transform);
		//GameManager.Instance.RemovePlayer(pde.PlayerGo);
		pde.PlayerGo.SetActive(false);
	}

	private void OnDestroy()
	{
		PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
	}
}
