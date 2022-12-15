using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Events;
//using UnityEditor.Rendering.LookDev;
//using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DashAdvanced : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool stopGravityWhileDashing = true;
    [SerializeField] private bool isInvincibileWhileDashing = true;
    [SerializeField] private bool canMoveWhileDashing = false;
    private bool isFacingRight;
    private bool isDashing;
    private bool onControlOverride;
    private bool onGigaChadMode;
    [Header("E1")]
    [SerializeField] private float dashingDistace = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private BoxCollider bcollider; // here
    [Header("E2")]
    [SerializeField] private float airDashingDistace = 24f;
    [SerializeField] private float airDashingDuration = 0.2f;
    [Header("E3")]
    [SerializeField] private float dashUpAngle = 90f;
    [SerializeField] private float dashUpAngleRange = 20;
    [SerializeField] private bool canDashDown = false;
    private bool currentCanDashDown;
    [SerializeField] private float dashDownAngle = -90f;
    [SerializeField] private float dashDownAngleRange = 20;
    [Header("E4")]
    [SerializeField] private float deadZoneAngle = -90;
    [SerializeField] private float deadZoneAngleRange = 90;
    [Header("Extra")]
    [SerializeField] private float dashingActivationCooldown = 1f;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject CharacterGlow;
    [Header("Sounds")]
    [SerializeField] private PlayerSoundManager playerSoundManager;
    private Vector3 direction;
    private Vector3 velocity;
    [SerializeField] private PlayerMovement movement;
    private PlayerHealth health;
    private float currentDashingDistace;
    private float currentDashingDuration;
    private float gravity;
    public UnityEvent dashEvent;

    enum DashType { E1_BasicDash, E2_TwoStateDash, E3_AdvancedDash, E4_GigaChadDash }
    [SerializeField] private DashType dashType;

    //#region Editor
    //[CustomEditor(typeof(DashAdvanced))]
    //public class DashAdvancedEditor : Editor
    //{
    //    DashAdvanced dash;
    //    public float dashingDistac;
    //    public override void OnInspectorGUI()
    //    {
    //        dash = (DashAdvanced)target;
    //        base.OnInspectorGUI();
    //        switch (dash.dashType)
    //        {
    //            case DashType.E1_BasicDash:
    //                DrawDetailsE1();
    //                break;
    //            case DashType.E2_TwoStateDash:
    //                DrawDetailsE2();
    //                break;
    //            case DashType.E3_AdvancedDash:
    //                DrawDetailsE3();
    //                break;
    //            case DashType.E4_GigaChadDash:
    //                DrawDetailsE4();
    //                break;
    //        }
    //    }

    //    private void DrawDetailsE1()
    //    {
    //        EditorGUILayout.LabelField("Details E1", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        GUILayout.Label("Distace", GUILayout.MaxWidth(100));
    //        //dash.SetDashingDistance(EditorGUILayout.FloatField(dash.dashingDistace));
    //        dashingDistac = EditorGUILayout.FloatField("", dashingDistac);
    //        DashAdvanced.dashingDistace = dashingDistac;
    //        EditorGUILayout.LabelField("Duration", GUILayout.MaxWidth(100));
    //        dash.dashingDuration = EditorGUILayout.FloatField(dash.dashingDuration);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE2()
    //    {
    //        DrawDetailsE1();
    //        EditorGUILayout.LabelField("Details E2", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("AirDistace", GUILayout.MaxWidth(100));
    //        dash.airDashingDistace = EditorGUILayout.FloatField(dash.airDashingDistace);
    //        EditorGUILayout.LabelField("AirDuration", GUILayout.MaxWidth(100));
    //        dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE3()
    //    {
    //        DrawDetailsE2();
    //        EditorGUILayout.LabelField("Details E3", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.LabelField("Up Angle", GUILayout.MaxWidth(100));
    //        dash.dashUpAngle = EditorGUILayout.FloatField(dash.dashUpAngle);
    //        EditorGUILayout.LabelField("Down Angle", GUILayout.MaxWidth(100));
    //        dash.dashDownAngle = EditorGUILayout.FloatField(dash.dashDownAngle);
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    private void DrawDetailsE4()
    //    {
    //        DrawDetailsE3();
    //        EditorGUILayout.LabelField("Details E4", EditorStyles.boldLabel);
    //        EditorGUILayout.BeginHorizontal();
    //        EditorGUILayout.EndHorizontal();
    //    }
    //    //private void TextIO(string title, float widthSpacing, DashAdvanced d)
    //    //{
    //    //    EditorGUILayout.LabelField(title, GUILayout.MaxWidth(widthSpacing));
    //    //    dash.airDashingDuration = EditorGUILayout.FloatField(dash.airDashingDuration);
    //    //}
    //}
    ////#endregion

    public void DashWithJoystick(InputAction.CallbackContext context)
    {
        if (enabled == false || GameManager.Instance.GameIsPaused == true || GameManager.Instance.AcceptPlayerInput == false || (!canDash && isDashing)) return;
        if (canDash && !isDashing)
        {
            CheckDashType();
        }
    }

    public void SetVelocity(Vector2 nVel)
    {
        velocity = nVel;
    }

    public void CheckDashWithJoystickDirection(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GameIsPaused == true || GameManager.Instance.AcceptPlayerInput == false || (!canDash && isDashing)) return;
        Flip();
        direction = context.ReadValue<Vector2>().normalized;
    }
    private void Start()
    {
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        currentCanDashDown = canDashDown;
        movement = GetComponent<PlayerMovement>();
        health = GetComponent<PlayerHealth>();
        gravity = movement.DownwardForce;
       // tr.time = dashingDuration;
    }
    //void Update()
    //{
    //    //DashWithKeyboard();
    //}

    private void Update()
    {
        if (GameManager.Instance.GameIsPaused == true || GameManager.Instance.AcceptPlayerInput == false) return;
        if (isDashing)
        {
            Debug.DrawLine(transform.position + new Vector3(0f, 0.5f, -0.3f), transform.position + new Vector3(0f, 0.5f, -0.3f) + velocity * Time.deltaTime, Color.red, 5);
            if (canMoveWhileDashing && !stopGravityWhileDashing) transform.position += velocity * Time.deltaTime;
            else if (canMoveWhileDashing) transform.position += (velocity - new Vector3(0f, movement.Velocity.y, 0f)) * Time.deltaTime;
            else if (!stopGravityWhileDashing) transform.position += (velocity - new Vector3(movement.Velocity.x, 0f, 0f)) * Time.deltaTime;
            else transform.position += (velocity - (Vector3)movement.Velocity) * Time.deltaTime;
        }
        //velocity = new Vector3(0, velocity.y, velocity.z);
    }
    private void CheckDashType()
    {
        switch (dashType)
        {
            case DashType.E1_BasicDash:
                SetDirection();
                break;
            case DashType.E2_TwoStateDash:
                CheckIfGrounded();
                SetDirection();
                break;
            case DashType.E3_AdvancedDash:
                CheckIfGrounded();
                SetDirectionWithControlOverride();
                break;
            case DashType.E4_GigaChadDash:
                onGigaChadMode = true;
                CheckIfGrounded();
                SetDirectionWithControlOverride();
                break;
        }

    }
    private IEnumerator BasicDashAction()
    {
        StartDashProtocol();
        velocity = new Vector3(direction.x * currentDashingDistace, 0f, 0f);
        Debug.DrawLine(transform.position + new Vector3(0f, 0.5f, 0.3f), transform.position + new Vector3(0f, 0.5f, 0.3f) + direction * currentDashingDistace*currentDashingDuration, Color.green, 5);

        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        CharacterGlow.SetActive(false);
        canDash = true;
    }
    private void StartDashProtocol()
    {
        //currentDashingDuration *= 2;
        playerSoundManager.PlayerDashSound();
        CheckForCollision();
        canDash = false;
        dashEvent.Invoke();
        tr.emitting = true; //See variable TrailRenderer tr
        gameObject.GetComponent<AfterImg>().StartTrail();
        CharacterGlow.SetActive(true);
        if (stopGravityWhileDashing)
        {
            movement.DownwardForce = 0f;
        }
        if (isInvincibileWhileDashing)
        {
            health.SetInvincible(true);
        }
        isDashing = true;
    }
    private void EndDashProtocol()
    {
        isDashing = false;
        tr.emitting = false; //See variable TrailRenderer tr
        currentDashingDistace = dashingDistace;
        currentDashingDuration = dashingDuration;
        currentCanDashDown = canDashDown;
        movement.DownwardForce = gravity;
        onControlOverride = false;
        health.SetInvincible(false);
    }
    private void SetDirection()
    {
        if (isFacingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
        StartCoroutine(BasicDashAction());
    }
    private void SetDirectionWithControlOverride()
    {
        float angle;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // -90 degrees
        if(direction.x != 0 && direction.y != 0)
        {
            onControlOverride = true;
            if (angle >= dashUpAngle - dashUpAngleRange && angle <= dashUpAngle + dashUpAngleRange)
            {
                direction = Vector3.up;
                StartCoroutine(UpDashAction());
                return;
            }
            else if (currentCanDashDown && angle >= dashDownAngle - dashDownAngleRange && angle <= dashDownAngle + dashDownAngleRange)
            {
                direction = Vector3.down;
                StartCoroutine(UpDashAction());
                return;
            }
            else if(onGigaChadMode && (direction.y >= 0|| currentCanDashDown))
            {
                StartCoroutine(GigaChadDashAction());
                return;
            }
        }
        SetDirection();
    }
    private IEnumerator UpDashAction()
    {
        StartDashProtocol();
        velocity = new Vector3(0f, direction.y * currentDashingDistace - movement.Velocity.y, 0f);
        Debug.DrawLine(transform.position + new Vector3(0f, 0.5f, 0.3f), transform.position + new Vector3(0f, 0.5f, 0.3f) + velocity * currentDashingDuration, Color.green, 5);
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        CharacterGlow.SetActive(false);
        canDash = true;
    }
    private IEnumerator GigaChadDashAction()
    {
        StartDashProtocol();
       // movement.Velocity = new Vector2();
        velocity = new Vector3(direction.x * currentDashingDistace, direction.y * currentDashingDistace, 0f);
        Debug.DrawLine(transform.position + new Vector3(0f, 0.5f, 0.3f), transform.position + new Vector3(0f, 0.5f, 0.3f) + velocity * currentDashingDuration, Color.green, 5);
        yield return new WaitForSeconds(currentDashingDuration);
        EndDashProtocol();
        yield return new WaitForSeconds(dashingActivationCooldown);
        CharacterGlow.SetActive(false);
        canDash = true;
    }
    private void CheckIfGrounded()
    {
        if (!movement.IsGrounded)
        {
            currentCanDashDown = true;
            currentDashingDistace = airDashingDistace;
            currentDashingDuration = airDashingDuration;
        }
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
        Debug.DrawLine(transform.position + new Vector3(0f, 0.5f, 0), transform.position + new Vector3(0f, 0.5f, 0) + direction * (currentDashingDistace * currentDashingDuration + bcollider.size.x), Color.blue, 5);
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0), direction, out hit, currentDashingDistace*currentDashingDuration + bcollider.size.x, collisionLayer)) //10 is a the number that make dash distance works correct 
        {
            if (onControlOverride)
            {
                currentDashingDistace = hit.distance / currentDashingDuration - bcollider.size.x / currentDashingDuration;
                currentDashingDistace = Mathf.Abs(currentDashingDistace);
            }
            else
            {
                //currentDashingDistace = hit.distance / currentDashingDuration - 2 * Mathf.Abs(movement.Velocity.x) - 5;
                currentDashingDistace = hit.distance / currentDashingDuration - Mathf.Abs(movement.Velocity.x) - bcollider.size.x / currentDashingDuration;
                //currentDashingDuration = currentDashingDuration * ((hit.distance / currentDashingDuration - 2 * Mathf.Abs(movement.Velocity.x)-7) / dashingDistace);
            }
        }
    }
    //private void CheckForCollision2()
    //{
    //    RaycastHit hit;
    //    if (Physics.BoxCast(transform.position, bcollider.size/2, direction, out hit, transform.rotation, currentDashingDistace * currentDashingDuration + bcollider.size.x, movement.CollisionLayer)) //10 is a the number that make dash distance works correct 
    //    {
    //        if (onControlOverride)
    //        {
    //            currentDashingDistace = hit.distance / currentDashingDuration - Mathf.Sqrt(Mathf.Pow(movement.Velocity.x, 2) + Mathf.Pow(movement.Velocity.y, 2)) - Mathf.Abs(movement.Velocity.x) - Mathf.Abs(movement.Velocity.y);
    //            currentDashingDistace = Mathf.Abs(currentDashingDistace);
    //        }
    //        else
    //        {
    //            currentDashingDistace = hit.distance / currentDashingDuration - bcollider.size.x / currentDashingDuration;
    //            //currentDashingDuration = currentDashingDuration * ((hit.distance / currentDashingDuration - 2 * Mathf.Abs(movement.Velocity.x)-7) / dashingDistace);
    //        }
    //    }
    //}
    public bool GetISFacingRight()
    {
        return isFacingRight;
    }
}
