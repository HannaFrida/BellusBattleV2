using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private CameraFocus CF;
    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;
    [SerializeField] private AudioSource playerDeathSound;
    [SerializeField] private VisualEffect bloodSplatter;
    [SerializeField] private VisualEffect poisoned;
    private PlayerMovement pm;
   

    private float health = 1;
    private bool isInvinsable=false;

    [SerializeField] Transform deathPosition;

    public float Health { get => health; }

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
        CF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFocus>();
        poisoned.gameObject.SetActive(false);
        UnkillPlayer();
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

    public void StopPoisoned()
    {
        poisoned.gameObject.SetActive(false);
        poisoned.Stop();

    }

    public void KillPlayer()
    {
        //Debug.Log("jjajajajaj" + " " + gameObject.GetComponent<PlayerDetails>().playerID + " i am dEAD " + Health);
        //CF.RemoveTarget(gameObject.transform);
        //gameObject.transform.position = deathPosition.position;
        GameManager.Instance.PlayerDeath(gameObject);
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
        Debug.Log("dufucjhedfjsujsuj" + " " + gameObject.GetComponent<PlayerDetails>().playerID + " i am dEAD aaginsss " + Health);

        /*
        boxCollider.enabled = false;
        rightArm.SetActive(false);
        skr.enabled = false;
        */
        //gunMesh.enabled = false;
        //grenadeMesh.enabled = false;
    }

    public void UnkillPlayer()
    {
        GameManager.Instance.RestorePLayer(gameObject);
        health = 1f;
        skr.enabled = true;
        anime.enabled = true;
        hips.SetActive(false);
        hips.SetActive(true);
        hips.transform.position = Vector3.zero;
        hips.SetActive(false);
        boxCollider.enabled = true;
        if (pm != null)
        {
            pm.enabled = true;
        }
        dash.enabled = true;
        /*
        boxCollider.enabled = true;
        rightArm.SetActive(true);
        skr.enabled = true;
        */
        //gunMesh.enabled = false;
        //grenadeMesh.enabled = false;
        //swordMesh.enabled = true;
        //ppV1.isHoldingWeapon = false;
    }
    
}
