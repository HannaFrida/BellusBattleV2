using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayerHealth : MonoBehaviour
{
    //public delegate void OnGameOver();
    //public static event OnGameOver onGameOver;
    [SerializeField] private AudioSource playerDeathSound;
    [SerializeField] private VisualEffect bloodSplatter;
    [SerializeField] private VisualEffect poisoned;
    [SerializeField] private VisualEffect lighting;
    [SerializeField] private VisualEffect fire;
    private PlayerMovement pm;
   

    private float health = 1;
    private bool isInvinsable;
    private bool isAlive = true;

    [SerializeField] Transform deathPosition;

    public float Health { get => health; }

    public bool IsAlive
    {
        get => isAlive;
    }

    //USCH
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private SkinnedMeshRenderer skr;
    [SerializeField] private GameObject hips;
    [SerializeField] private Animator anime;
    [SerializeField] private DashAdvanced dash;


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void OnLevelWasLoaded(int level)
    {
        poisoned.gameObject.SetActive(false);
        UnkillPlayer();

        gameObject.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }


    public void TakeDamage(float damage)
    {
        //Debug.Log("ajajaj" + " " + gameObject.GetComponent<PlayerDetails>().playerID);
        //if(isInvinsable) return;
        health -= damage;
        if (health <= 0)
        {
            //Debug.Log("ajajaj" + " " + gameObject.GetComponent<PlayerDetails>().playerID + " dEAD " + Health);
            KillPlayer();
            playerDeathSound.Play();
            //onGameOver.Invoke();
        }
    }
    public void SetInvincible( bool value)
    {
        isInvinsable = value;
    }

    public void PlayPoisoned()
    {
        poisoned.gameObject.SetActive(true);
        //poisoned.Play();

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
        GetComponent<PlayerDetails>().Rumble(0.5f,0.5f);
        boxCollider.enabled = false;
        bloodSplatter.Play();
        hips.SetActive(true);
        anime.enabled = false;
        if (pm != null)
        {
            pm.enabled = false;
        }
        
        dash.enabled = false;
        if (gameObject.GetComponentInChildren<Gun>() != null)
        {
            gameObject.GetComponentInChildren<Gun>().Drop();
        }
    
    }

    public void UnkillPlayer()
    {
        isAlive = true;
        health = 1f;
        skr.enabled = true;
        anime.enabled = true;
        hips.SetActive(false);
        hips.SetActive(true);
        hips.transform.position = Vector3.zero;
        hips.SetActive(false);
        boxCollider.enabled = true;
        dash.enabled = true;
      
    }
    
}
