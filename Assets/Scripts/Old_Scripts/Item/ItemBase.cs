using UnityEngine;
using System;
using Random = System.Random;


namespace ItemNamespace
{
    // Creates the ScriptableObject function for the Item objects.
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create new Weapon")]

    public class ItemBase : ScriptableObject
    {
        [SerializeField] private ItemType itemType;
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private string itemName;
        //[SerializeField] private string description;
        [SerializeField] private int damage; // only interesting if weapon
        [SerializeField] private float range;
        [SerializeField] private float speed;
        //[SerializeField] private float stamina;
        //[SerializeField] private int healAmount; // only interesting if food
        //[SerializeField] private Sprite sprite; // the icon shown when interacting with the item
        //[SerializeField] private bool stackable; // if the item can be stacked in the inventory or the player bar
        [SerializeField] private Mesh mesh; // the mesh for the item in the world
        [SerializeField] private Material material; // the material for the item in the world
        [SerializeField] private string usageParticle; // the object with base behavior script for the item
        [SerializeField] private string itemBaseBehaviorScriptName; // the object with base behavior script for the item
        //[SerializeField] private int protection;
        [SerializeField] private float speedMultiplierWhenUsingItem;
        [SerializeField] private float staggerStaminaDrain;// only interesting if weapon
        [SerializeField] private GameObject particle;// only interesting if weapon

        // Firearm stuff
        [SerializeField, Tooltip("How accurate the firearm is.")] private float inaccuracy;
        [SerializeField, Tooltip("Rounds per second.")] private float fireRate;
        [SerializeField, Tooltip("Ammunition of the firearm.")] private int ammo;
        [SerializeField, Tooltip("What projectile is being fired.")] private GameObject projectile;
        [SerializeField, Tooltip("Where the projectile is fired from.")] private GameObject projectileOrigin;
        [SerializeField, Tooltip("The amount of force placed on the projectile.")] private float projectileForce = 100.0f;
        
        private Projectile _projectile;
        private float _nextTimeToFire;
        private bool _isFiring;
        //[SerializeField] public Aim aim; // test to make bullet shoot in correct direction
        private readonly Random _random = new();

        private void Awake()
        {
            //aim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Aim>();
        }


        //Returns the the hit particle that will play when used
        public GameObject GetParticle => particle;

        //Returns the amounts of hit for stagger with this item
        public float GetStaggerStaminaDrain => staggerStaminaDrain;

        public float GetSpeedMultiplierWhenUsingItem => speedMultiplierWhenUsingItem;
        // Returns armor level
        //public int GetProtection => protection;

        // Returns damage output when used
        public int GetDamage => damage;

        // Returns range when used
        public float GetRange => range;

        // Returns speed when used
        public float GetSpeed => range;

        // Returns stamina when used
        //public float GetStamina => stamina;

        // Returns name of the item
        public string GetName => itemName;

        // Returns the amount healed when using the item
        //public int GetHealAmount => healAmount;

        // Returns the description of the item
        //public string GetDescription => description;

        // Returns the 2D image for the item
        //public Sprite GetSprite => sprite;

        // Returns this type
        public ItemType GetItemType => itemType;

        // Returns this type
        public WeaponType GetWeaponType => weaponType;

        //Returns mesh
        public Mesh GetMesh => mesh;

        //Returns material
        public Material GetMaterial => material;

        //Returns name of script with this items behavior
        public String GetItemBaseBehaviorScriptName => itemBaseBehaviorScriptName;

        //Returns the particle effect 
        public String GetUsageParticle => usageParticle;

        // Contains the different item type, add a new line to the enum in order to add an item type.
        public enum ItemType
        {
            Weapon
        }

        public enum WeaponType
        {
            None,
            Sword,
            Pistol,
            AR
        }
    }
}