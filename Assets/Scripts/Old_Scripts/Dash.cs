using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class Dash : MonoBehaviour
{
    enum DashType { FullAngleDash , FourFixedAnglesDash, HorizantalAngleDash }
    [SerializeField] DashType dashType;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingActivationCooldown = 1f;
    [SerializeField] private float dashingInvincibilityDuration = 1f;
    [SerializeField] private AudioSource dashSound;
    //[SerializeField] private TrailRenderer tr; // these variable makes visual effect

    private Vector3 velocity;
    private PlayerMovement movement;
    private PlayerHealth health;
    private float currentDashingDistace;
    private float currentDashingDuration;
    private float gravity;
    Vector3 direction;

    private bool isDashing;
    private bool isFacingRight = true;

    public UnityEvent dashEvent;

    public void ResetValues()
    {
        isDashing = false;
        canDash = true;
    }

    private void Start()
    {
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        gravity = movement.DownwardForce;
    }
    
    void Update()
    {
        Flip();
        //DashWithKeyboard();
    }
    
    private void FixedUpdate()
    {
        if (isDashing)
        {
            transform.position += velocity * Time.deltaTime;
            return;
        }
        velocity = new Vector3(0, velocity.y, velocity.z);
    }
    private void DashWithKeyboard()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // INPUT
        {
            StartCoroutine(DashAction());
        }
    }
    public void DashWithJoystick(InputAction.CallbackContext context)
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(DashAction());
        }
    }
    public void CheckDashWithJoystickDirection(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        if (direction.x == 0 && direction.y == 0)
        {
            if(isFacingRight) direction = Vector2.right;
            else direction = Vector2.left;
        }
        else if (isFacingRight)
        {
            direction = Vector2.right * direction;
            direction.Normalize();
        }
        else
        {
            direction = Vector2.left * direction;
            direction.Normalize();
        }
    }

    private IEnumerator DashAction()
    {
        StartDashProtocol();
        if (stopGravityWhileDashing) movement.DownwardForce = -15f; // should be 0???
        Debug.Log(movement.Velocity.x);
        if(isFacingRight)velocity = new Vector3(currentDashingDistace - movement.Velocity.x, direction.y*4 * currentDashingDistace, 0f);
        else velocity = new Vector3(-currentDashingDistace - movement.Velocity.x, direction.y*4 * currentDashingDistace, 0f);

        //tr.emitting = true; //See variable TrailRenderer tr
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        canDash = true;

    }
 
    private void Flip()
    {
        if (isFacingRight && movement.Velocity.x < 0f || !isFacingRight && movement.Velocity.x > 0f)
        {
            isFacingRight = !isFacingRight;
        }
    }
    private void CheckForCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, currentDashingDistace/4, movement.CollisionLayer)) //4 is a the number that make dash distance works correct 
        {
            if (hit.distance * 4 < currentDashingDistace / 4)
            {
                currentDashingDistace = 0;
                return;
            }
            currentDashingDistace = hit.distance * 4 - 1f; /// 4 is a the number that make dash distance works correct // 0.5f är Players halv storlek
        }
    }
    private void StartDashProtocol()
    {
        dashSound.Play();
        CheckForCollision();
        canDash = false;
        isDashing = true;
        //StartCoroutine(Invincibility());
        dashEvent.Invoke();
    }
    private void EndDashProtocol()
    {
        //tr.emitting = false; //See variable TrailRenderer tr
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        movement.DownwardForce = gravity;
        isDashing = false;
    }


}
