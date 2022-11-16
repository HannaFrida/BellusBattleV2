using UnityEngine;

public class PickUpListener : MonoBehaviour
{
	private void Start()
	{ 
		PickUpEvent.RegisterListener(OnPickUp);
	}

    private void OnPickUp(PickUpEvent pue)
    {
        
    }

    private void OnDestroy()
    {
        PickUpEvent.UnregisterListener(OnPickUp);
    }
}
