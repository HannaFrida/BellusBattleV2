using System.Collections;
using ItemNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSwordBehaviour : ItemBaseBehaviour
{
    public GameObject Sword;
    Vector3 pos;
    Vector3 rotation;
    public bool canAttack = true;
    public bool isCurrentlyAttacking;
    public float cooldown = 3;
    public Transform ShootPoint;

    public bool attackLocked;
    private GameObject rayCastPosition;
    private Camera mainCamera;
    /// <summary>
    ///  LÄGG TILL ANIMATOR NÄR VI HAR DEN 
    /// </summary>
    //private Animator animator;


    public void Awake()
    {
        canAttack = true;
        //rayCastPosition = gameObject.transform.Find("rayCastPosition").gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Sword = GameObject.FindGameObjectWithTag("Sword");
        //animator = gameObject.transform.Find("VikingWarrior").GetComponent<Animator>();
    }

    IEnumerator AddAttackCooldown()
    {
        Debug.Log(pos);
        Sword.transform.position = pos;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
        
    }
    public override void Use(ItemBase itemBase)
    {
        pos = Sword.transform.position;
        if (canAttack)
        {
            //Sword.transform.position += ShootPoint.transform.up * 1.02f;
            Debug.Log("Swosh");
            isCurrentlyAttacking = true;
            canAttack = false;
            StartCoroutine(AddAttackCooldown());
            Debug.Log(pos);
            isCurrentlyAttacking = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && isCurrentlyAttacking)
        {
            other.gameObject.transform.Find("revolver").gameObject.SetActive(false);
            other.gameObject.transform.Find("Sword").gameObject.SetActive(true);
            Debug.Log("Dropeed");
        }
    }

    public override void StopAnimation()
    {
        
    }

    //Waits the length of the animation before letting the player attack again.
    IEnumerator WaitToAttack(float time)
    {
        // Used to lock the ability to swap between items while attacking
        attackLocked = true;

        yield return new WaitForSeconds(time / 2);
        Collider[] hits = Physics.OverlapSphere(transform.position, belongingTo.GetRange / 2, LayerMask.GetMask("Enemy"));
        if (hits.Length > 0)
        {
            Collider enemy = null;
            float closest = Mathf.Infinity;
            foreach (Collider hit in hits)
            {
                if (Vector3.Distance(hit.transform.position, gameObject.transform.position) < closest)
                {
                    closest = Vector3.Distance(hit.transform.position, gameObject.transform.position);
                    enemy = hit;
                }
            }
            
        }

        yield return new WaitForSeconds(time / 2);
        //animator.SetLayerWeight(animator.GetLayerIndex("Sword Attack"), 0);
        canAttack = true;

        // Used to lock the ability to swap between items while attacking
        attackLocked = false;
    }
}