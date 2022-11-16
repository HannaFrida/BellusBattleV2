using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Item : MonoBehaviour{
	[SerializeField] [Tooltip("")] 
	private float speed = 10;
	[SerializeField] [Tooltip("")]
	private GameObject[] weaponModels;

	public enum WeaponTypes{
		shotgun,
		pistol,
		grenade,
	}

	private void Start(){
		//Initialize a model of the weapon here.
	}

	private void Update(){
		Rotate();
	}

	private void Rotate(){
		transform.Rotate(Vector3.up * (speed * Time.deltaTime));
	}
}