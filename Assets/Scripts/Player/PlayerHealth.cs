using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
/*
* Author Martin Wallmark, Khaled Alraas
*/
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private AudioSource playerDeathSound;
    [SerializeField] private VisualEffect bloodSplatter;
    [SerializeField] private VisualEffect poisoned;
    [SerializeField] private VisualEffect lighting;
    [SerializeField] private VisualEffect fire;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Collider headCollider;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private SkinnedMeshRenderer skr;
    [SerializeField] private GameObject hips;
    [SerializeField] private Animator anime;
    [SerializeField] private DashAdvanced dash;
    private PlayerMovement pm;
   

    private float health = 1;
    private bool isAlive = true;

    [SerializeField] Transform deathPosition;

    public float Health { get => health; }

    public bool IsAlive
    {
        get => isAlive;
    }
    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        UnkillPlayer();
    }

    private void OnLevelWasLoaded(int level)
    {
        poisoned.gameObject.SetActive(false);
        UnkillPlayer();

        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap(GetComponent<PlayerDetails>().ChosenActionMap);
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            KillPlayer();
            playerDeathSound.Play();
        }
    }
   

    public void PlayPoisoned()
    {
        poisoned.gameObject.SetActive(true);
    }
    public void PlayLighting()
    {
        lighting.gameObject.SetActive(true);

        lighting.Play();

    }
    public void PlayFire()
    {
        fire.gameObject.SetActive(true);
        fire.Play();
    }

    public void StopPoisoned()
    {
        poisoned.gameObject.SetActive(false);
        poisoned.Stop();

    }

    public void KillPlayer()
    {
        isAlive = false;
        GameManager.Instance.PlayerDeath(gameObject);
        headCollider.enabled = false;
        GetComponent<PlayerDetails>().Rumble(0.5f,0.5f);
        boxCollider.enabled = false;
        bloodSplatter.Play();
        hips.SetActive(true);
        anime.enabled = false;
        if (pm != null)
        {
            pm.ResetForces();
            pm.enabled = false;
        }
        
        dash.enabled = false;
        if (gameObject.GetComponentInChildren<Gun>() != null)
        {
            Gun gun = gameObject.GetComponentInChildren<Gun>();
            if (gun.canDrop == false)
            {
                gun.canDrop = true;
            }
            gun.Drop();
        }
    
    }

    public void UnkillPlayer()
    {
        isAlive = true;
        health = 1f;
        skr.enabled = true;
        headCollider.enabled = true;
        anime.enabled = true;
        hips.SetActive(false);
        hips.SetActive(true);
        hips.transform.localPosition = Vector3.zero;
        hips.SetActive(false);
        boxCollider.enabled = true;
        dash.enabled = true;
      
    }
    
}
