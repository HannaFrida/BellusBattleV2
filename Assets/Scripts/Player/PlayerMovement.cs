using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField, Range(1f, 25f)] private float moveSpeed;
    [SerializeField, Range(1f, 25f)] private float jumpForce;
    [SerializeField, Range(25f, 150f)] private float airDeceleration;
    [SerializeField, Range(25f, 150f)] private float groundDeceleration;
    [SerializeField, Range(10f, 300f)] private float airResistance;
    [SerializeField, Range(-100f, 0f)] private float downwardForce;
    [SerializeField, Range(1f, 50000f)] private float acceleration;
    [SerializeField, Range(-1f, -50000f)] private float fallForce;
   
    [Header("Jump & Edgecontrol")]
    [SerializeField, Range(0f, 1f)] private float doubleJumpDecreaser;
    [SerializeField, Range(-1f, 0f)] private float downwardInputBound;
    [SerializeField] private float edgeControlAmount;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private LayerMask oneWayLayer;
    
    [Header("Sound & VFX")]
    [SerializeField] private PlayerSoundManager playerSoundManager;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject doubleJumpVFX;

    [SerializeField] private PlayerInput playerInput;


    public PlayerInput PlayerInput  => playerInput;


    public UnityEvent jumpEvent;

    public LayerMask CollisionLayer
    {
        get => collisionLayer; 
    }

    public Vector2 Velocity
    {
        get => velocity; 
    }

    public float DownwardForce
    {
        get => downwardForce;
        set => downwardForce = value; 
    }

    public float FallForce
    {
        get => fallForce;
        set => fallForce = value;
    }

    public bool IsGrounded
    {
        get => CheckIsGrounded(); 
    }

    public bool IsMovedByPLatform
    {
        get => isMovedByPLatform;
        set => isMovedByPLatform = value;
    }

    private Vector2 velocity;
    private Vector2 externalForce;
    private Vector2 rayCastBottomLeft, rayCastBottomRight, rayCastTopRight, rayCastTopLeft;

    private Vector2 verticalRayOffset, horizontalRayOffset;
    

    private GameObject MuzzleFlashIns;

    private BoxCollider boxCollider;

    private int horizontalRayCount = 6; 
    private int verticalRayCount = 4;

    private float movementX, movementY;
    private float deceleration;
    
    private float coyoteTimer, bufferTimer, knockBackTimer;
    private float horizontalSkinWidth = 0.2f;
    private float verticalSkinWidth = 0.1f;
    private float downwardInput;
    private float verticalRaySpacing, horizontalRaySpacing;
    private float verticalRayLength, horizontalRayLength;
    private float movementAmount;
    private float initialSpeed;
    private float movementAnimationSpeed;
    private float playerHeight;

    private float knockBackTime = 0.2f;

    private bool hasJumpedOnGround, hasDoubleJump, hasCoyoteTime;
    
    
    private bool isMovingLeft, isMovingRight;
    private bool isStandingOnOneWayPlatform;
    private bool runBufferTimer;
    private bool hasJumpBuffer;
    private bool hasBeenKnockedBack;
    private bool isGrounded;
    private bool isMovedByPLatform;

    private void OnLevelWasLoaded(int level)
    {
        externalForce = Vector2.zero;
    }

    void Start()
    {
        initialSpeed = moveSpeed - 5; //Används för acceleration
        boxCollider = GetComponent<BoxCollider>();
        CalculateRayLength();
        playerHeight = verticalRayLength * 2;
        CalculateRaySpacing();
        DontDestroyOnLoad(gameObject);
        CalculateRaycastOffset();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded;
        UpdateRayCastOrgins();
        UpdateMovementForce();
        UpdateCoyoteTime();
        RunKnockbackTimer();
        
        if (isGrounded == false)
        {
            movementY = Mathf.MoveTowards(movementY, downwardForce, airResistance * Time.deltaTime);
            
        }

        if (isGrounded && velocity.y < 0)
        {

            
            playerSoundManager.PlayerLandSound();
           
            
            
            if (hasJumpBuffer)
            {
                Jump();
                hasJumpBuffer = false;
                runBufferTimer = false;

            }
            

        }
        velocity = new Vector2(movementX, movementY) + externalForce;
        JumpBuffer();

        if (velocity.y != 0)
        {
            
        }
        HandleVerticalCollisions(ref velocity);

        if (velocity.x != 0)
        {
            HandleHorizontalCollisions(ref velocity);
        }
        
        
        transform.Translate(velocity * Time.deltaTime);
        //Debug.Log(movementAmount);
        movementAnimationSpeed = movementX;
        if (isMovedByPLatform == true && movementAmount == 0f)
        {
            playerAnimator.SetFloat("Speed", 0f);
        }
        else
        {
            playerAnimator.SetFloat("Speed", movementX);
        }


        
        
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameIsPaused == true) return;

        downwardInput = ctx.ReadValue<Vector2>().y;
        movementAmount = ctx.ReadValue<Vector2>().x;
      
        if (ctx.ReadValue<Vector2>().x > 0.1f)
        {
            isMovingRight = true;
            isMovingLeft = false;
        }
        else if (ctx.ReadValue<Vector2>().x < -0.1f)
        {
            isMovingRight = false;
            isMovingLeft = true;
        }
        else if (ctx.ReadValue<Vector2>().x < 0.1f || ctx.ReadValue<Vector2>().x > -0.1f)
        {
            isMovingRight = false;
            isMovingLeft = false;
        }

        if(movementAmount < 0.1f && movementAmount > -0.1f)
        {
            movementAmount = 0f;
        }

        // Hold down to fall faster
        if (!IsGrounded && -downwardInput > 0.1f)
        {
            float temp = DownwardForce;
            temp = fallForce;
            DownwardForce = temp;
        }
        else
        {
            SetDownwardForce(downwardForce, DownwardForce);    
        }

    }

    private void AccessabilityoveDown() // Ska användas om man kör med onehand-mode!
    {
        if (downwardInput <= downwardInputBound && isStandingOnOneWayPlatform)
        {
            transform.position += Vector3.down * playerHeight;
            isStandingOnOneWayPlatform = false;
            return;
        }
    }

    public static void SetDownwardForce(float value, float downfroce)
    {
        float temp = downfroce;
        temp = value;
        downfroce = temp;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.GameIsPaused == true) return;

        if (ctx.started)
        {
            Jump();
        }
    }

    private void Jump()
    {
        float jumpDecreaser = 1f;
        if (downwardInput <= downwardInputBound && isStandingOnOneWayPlatform)
        {
            transform.position += Vector3.down * playerHeight;
            isStandingOnOneWayPlatform = false;
            return;
        }
        else if (isGrounded || hasCoyoteTime || hasDoubleJump)
        {
            if (isGrounded)
            {
                hasJumpedOnGround = true;
                playerSoundManager.PlayerJumpSound();


            }
            if (!hasCoyoteTime && hasDoubleJump)
            { 
                playerSoundManager.PlayerDoubleJumpSound();
                MuzzleFlashIns = Instantiate(doubleJumpVFX, transform.position, transform.rotation);
                Destroy(MuzzleFlashIns, 1.5f);
                StartCoroutine(VFXRemover());
                hasDoubleJump = false;
                jumpDecreaser = doubleJumpDecreaser;
            }
            movementY = jumpForce * jumpDecreaser;
            jumpEvent.Invoke();
        }
        else
        {
            
            runBufferTimer = true;
            bufferTimer = 0;
        }
    }

    private void ResetValuesOnGrounded()
    {
        coyoteTimer = 0;
        hasCoyoteTime = true;
        hasDoubleJump = true;
        hasJumpedOnGround = false;
    }
    private IEnumerator VFXRemover()
    {
        yield return new WaitForSeconds(1f);
        Destroy(MuzzleFlashIns);
    }

    private void JumpBuffer()
    {
        if (!runBufferTimer) return;
        bufferTimer += Time.deltaTime;
        if(bufferTimer <= jumpBufferTime)
        {
            hasJumpBuffer = true;
        }
        else
        {
            hasJumpBuffer = false;
            runBufferTimer = false;
        }
    }

    private void RunKnockbackTimer()
    {
        if (hasBeenKnockedBack == false) return;

        knockBackTimer += Time.deltaTime;

        if(knockBackTimer >= knockBackTime)
        {
            hasBeenKnockedBack = false;
            knockBackTimer = 0f;
        }
    }

    private void EdgeControl(RaycastHit hit)
    {
        float hitColliderBuffer = 0.1f; // Avståndet spelaren kommer att placeras över den träffade colliderns största y-värde
        float hitpointY = hit.point.y;
        Collider platformCollider = hit.collider;
        Bounds col = platformCollider.bounds;

        float colliderDif = col.max.y - hitpointY;
        //Debug.Log(colliderDif);

        if (colliderDif >= -.01f && colliderDif <= edgeControlAmount)
        {
            if (velocity.x < 0f)
            {
                transform.position = new Vector3(col.max.x, col.max.y + hitColliderBuffer, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(col.min.x, col.max.y + hitColliderBuffer, transform.position.z);
            }
        }
    }

    private void UpdateMovementForce()
    {
        if (hasBeenKnockedBack) return;

        if(movementAmount > 0.1f || movementAmount < -0.1f)
        {
            if (isMovingRight)
            {
                movementX = Mathf.MoveTowards(initialSpeed, moveSpeed, acceleration * Time.deltaTime);
            }
            if (isMovingLeft)
            {
                movementX = Mathf.MoveTowards(initialSpeed, moveSpeed, acceleration * Time.deltaTime);
            }
            movementX *= movementAmount;
        }
        else
        {
            if (isMovedByPLatform) return;
            movementX = Mathf.MoveTowards(movementX, 0, deceleration * Time.deltaTime);
        }
        if(externalForce.x != 0f)
        {
            externalForce.x = Mathf.MoveTowards(externalForce.x, 0, deceleration * Time.deltaTime);
        }
        if(externalForce.y != 0f)
        {
            externalForce.y = Mathf.MoveTowards(externalForce.y, 0, deceleration * Time.deltaTime);
        }
    }
    private bool CheckIsGrounded()
    {
        
        if (Physics.Raycast(boxCollider.bounds.center, Vector2.down, verticalRayLength, oneWayLayer))
        {
            isStandingOnOneWayPlatform = true;
            deceleration = groundDeceleration;
            return true;
        }
        if (Physics.Raycast(boxCollider.bounds.center, Vector2.down, verticalRayLength, groundLayer))
        {
            deceleration = groundDeceleration;
            isStandingOnOneWayPlatform = false;
            return true;
        }
        else
        {
            deceleration = airDeceleration;
            isStandingOnOneWayPlatform = false;
            return false;
        }
    }
    private void UpdateCoyoteTime()
    {
        if (isGrounded || !hasCoyoteTime) return;
 
        if (coyoteTimer > coyoteTime || hasJumpedOnGround)
        {
            hasCoyoteTime = false;
        }
        coyoteTimer += Time.deltaTime;
    }

    private void HandleVerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin;

            if (directionY == -1)
            {
                rayOrigin = rayCastBottomLeft + verticalRayOffset;
            }
            else
            {
                rayOrigin = rayCastTopLeft - verticalRayOffset;
            }
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * verticalRayLength, Color.red);
           
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, verticalRayLength, collisionLayer))
            {
                if(velocity.y < 0f)
                {
                    transform.position = new Vector2(transform.position.x, hit.collider.bounds.max.y);
                    ResetValuesOnGrounded();
                }
                velocity.y = 0f;
                movementY = 0f;
                return;
            }
            if (Physics.Raycast(rayOrigin, Vector2.up * directionY, out hit, verticalRayLength, oneWayLayer))
            {
                if (velocity.y > 0) return;

                if (velocity.y < 0f)
                {
                    transform.position = new Vector2(transform.position.x, hit.collider.bounds.max.y);
                    ResetValuesOnGrounded();
                }
                velocity.y = 0f;
                movementY = 0f;
                return;
            } 
        }
    }

    private void HandleHorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        for (int i = horizontalRayCount-1; i >= 0; i--)
        {
            Vector2 rayOrigin;
            if (directionX == -1)
            {
                rayOrigin = rayCastBottomLeft + horizontalRayOffset;
            }
            else
            {
                rayOrigin = rayCastBottomRight - horizontalRayOffset;
            }
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
     
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * horizontalRayLength, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector2.right * directionX, out hit, horizontalRayLength, collisionLayer))
            {
                if(i == 0)
                {
                    EdgeControl(hit);
                }
                velocity.x = 0;
                movementX = 0;
                return;
            }
        }
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private void UpdateRayCastOrgins()
    {
        Bounds bounds = boxCollider.bounds;
        rayCastBottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastTopLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastBottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastTopRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaycastOffset()
    {
        Bounds bounds = boxCollider.bounds;
        horizontalRayOffset = new Vector2((bounds.max.x - bounds.min.x) / 2, 0f);
        verticalRayOffset = new Vector2(0f, (bounds.max.y - bounds.min.y) / 2);
    }

    private void CalculateRayLength()
    {
        verticalRayLength = ((boxCollider.bounds.max.y - boxCollider.bounds.min.y) / 2f) + verticalSkinWidth;
        horizontalRayLength = ((boxCollider.bounds.max.x - boxCollider.bounds.min.x) / 2) + horizontalSkinWidth;
    }

    public void AddExternalForce(Vector2 force)
    {
        //Debug.Log("jdjada");
        hasBeenKnockedBack = true;
        knockBackTimer = 0f;
        movementY = force.y;
        movementX = force.x;
        
    }
    public void AddConstantExternalForce(Vector2 force)
    {
        externalForce = force;
    }

    public void AddForceFromMovingObject(Vector2 force)
    {
        movementY += force.y;
        movementX = force.x;
    }

}
