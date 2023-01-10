using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author Hanna Rudöfors
*/
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/New Weapon")]
public class WeaponData : ScriptableObject
{
   
    [Header("Info")]
    public new string name;

    [Header("Prefab")]
    public GameObject weaponPrefab;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    [SerializeField] public GameObject projectile;
    [SerializeField, Tooltip("The amount of force placed on the projectile.")]
    public float projectileForce;

    [Header("Spread")]
    public bool usesSpread;
    public int spreadFactor;
    public int pelletCount;

    [Header("Ammo")]
    public int currentAmmo;
    public int initialAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;

    [Header("Sounds")]
    [SerializeField, Tooltip("Sound made when picking up weapon")]
    public AudioSource pickupSound;
    [SerializeField, Tooltip("Sound made when using weapon")]
    public AudioSource shootAttackSound;

    [Header("VFX")]
    [SerializeField] private GameObject muzzleFlashGameObject;
    [SerializeField] private GameObject despawnVFX;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair;

    // Getters
    public int Ammo { get => currentAmmo; }

    
    public GameObject MuzzleFlashGameObject { get => muzzleFlashGameObject; }

    public GameObject DespawnVFX { get => despawnVFX; }

    public GameObject Crosshair { get => crosshair; }

}
