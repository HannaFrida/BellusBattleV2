using UnityEngine;

//Collection class containing all projectiles.
public class Projectile : MonoBehaviour{
	protected GameObject shooter;
	protected int shooterID;
	protected float damage;
	protected string weaponName;

	public void SetShooter(GameObject playerGo)
	{
		shooter = playerGo;
	}

	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	public void SetWeaponName(string name)
    {
		weaponName = name;
    }
	public void SetShooterID(int id)
    {
		shooterID = id;
		Debug.Log("id is: " + shooterID);
    }
}
