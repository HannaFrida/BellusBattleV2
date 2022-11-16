using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CameraTest : MonoBehaviour
{
	[SerializeField] [Tooltip("The maximum distance from the center of this game object it can spawn.")]
	private int max = 10;
	[SerializeField] [Tooltip("The camera which should follows its targets.")] 
	public CameraFocus focus;
	
	private List<GameObject> _players = new();
	private readonly Random _random = new();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			go.transform.parent = gameObject.transform;
			Vector3 pos = transform.position;
			go.transform.position = new Vector3(pos.x + _random.Next(-max, max), pos.y + _random.Next(0, max), 0);
			_players.Add(go);
			focus.AddTarget(go.transform);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (_players.Count == 0) {return;}
			int i = _random.Next() % _players.Count;
			GameObject go = _players[i];

			_players.Remove(go);
			focus.RemoveTarget(go.transform);
			Destroy(go);
		}
	}
}
