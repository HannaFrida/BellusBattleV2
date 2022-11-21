using UnityEngine;

public class PlayerSpawnListener : MonoBehaviour
{
	[SerializeField, Tooltip("All possible spawn locations.")]
	private Transform[] spawnLocations;
	
	private void Start()
	{
		PlayerSpawnEvent.RegisterListener(SpawnPlayer);
	}

	private void SpawnPlayer(PlayerSpawnEvent pse)
	{
		GameObject go = pse.PlayerInput.gameObject;
		go.GetComponent<PlayerDetails>().playerID = pse.PlayerInput.playerIndex + 1;
		//GameManager.Instance.AddPlayer(pse.Player);
		pse.PlayerInput.gameObject.GetComponent<PlayerDetails>().startPos = spawnLocations[pse.PlayerInput.playerIndex].position;
	}

	private void OnDestroy()
	{
		PlayerSpawnEvent.UnregisterListener(SpawnPlayer);
	}
}
