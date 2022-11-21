using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirearmTest : MonoBehaviour{
	[SerializeField] [Tooltip("All firearms to test fire.")]
	private List<GameObject> firearms;

	private List<Weapon> _bfs;
	private KeyCode[] _keyCodes;

	bool hasFired; // Används för att kunna använda med det nya Input Systemet

	private void Start()
	{
		_bfs = new List<Weapon>(firearms.Count);
		_keyCodes = new KeyCode[9];
		
		for (int i = 0; i < firearms.Count; i++)
		{
			_bfs.Add(firearms[i].GetComponent<Weapon>());
			_keyCodes[i] = GetKeyCodes(i);
		}
	}

	private void Update()
	{
        
		for (int i = 0; i < firearms.Count; i++)
		{
			if (firearms[i] == null)
			{
				firearms.RemoveAt(i);
				_bfs.RemoveAt(i);
			}
			if (hasFired)
			{
				_bfs[i].Fire();
			}

		}
	}

	void OnFire(InputAction.CallbackContext ctx)
    {
		if (ctx.started)
		{
			hasFired = true;
		}
	}

	private KeyCode GetKeyCodes(int index)
	{
		switch(index)
		{
			case 0:
				return KeyCode.Alpha1;
			case 1:
				return KeyCode.Alpha2;
			case 2:
				return KeyCode.Alpha3;
			case 3:
				return KeyCode.Alpha4;
			case 4:
				return KeyCode.Alpha5;
			case 5:
				return KeyCode.Alpha6;
			case 6:
				return KeyCode.Alpha7;
			case 7:
				return KeyCode.Alpha8;
			case 8:
				return KeyCode.Alpha9;
			case 9:
				return KeyCode.Alpha0;
		}

		//Has to return a keycode, can't return null.
		return KeyCode.Q;
	}
}
