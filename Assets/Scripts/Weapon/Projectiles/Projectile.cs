using UnityEngine;

//Collection class containing all projectiles.
public class Projectile : MonoBehaviour{
	protected GameObject Shooter;
	protected float damage;

	public void SetShooter(GameObject playerGo)
	{
		Shooter = playerGo;
	}

	public void SetDamage(float damage)
	{
		this.damage = damage;
	}
}
