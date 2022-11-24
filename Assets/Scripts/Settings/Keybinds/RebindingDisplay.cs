using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindingDisplay : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private TMP_Text bindingJumpDisplayNameText = null;
    [SerializeField] private GameObject startJumpRebindObject = null;

    [Header("Dodging")]
    [SerializeField] private InputActionReference dodgeAction = null;
    [SerializeField] private TMP_Text bindingDodgeDisplayNameText = null;
    [SerializeField] private GameObject startDodgeRebindObject = null;

    [Header("Shooting")]
    [SerializeField] private InputActionReference shootAction = null;
    [SerializeField] private TMP_Text bindingShootDisplayNameText = null;
    [SerializeField] private GameObject startShootRebindObject = null;

    [Header("Moving")]
    [SerializeField] private InputActionReference moveAction = null;
    [SerializeField] private TMP_Text bindingMoveDisplayNameText = null;
    [SerializeField] private GameObject startMoveRebindObject = null;

    [Header("Aiming")]
    [SerializeField] private InputActionReference aimAction = null;
    [SerializeField] private TMP_Text bindingAimDisplayNameText = null;
    [SerializeField] private GameObject startAimRebindObject = null;

    [Header("AimingOverride")]
    [SerializeField] private InputActionReference aimOverrideAction = null;
    [SerializeField] private TMP_Text bindingAimOverrideDisplayNameText = null;
    [SerializeField] private GameObject startAimOverrideRebindObject = null;

    [Header("General")]
    [SerializeField] private PlayerMovement playerMovement = null;
    [SerializeField] private GameObject waitingForInputObject = null;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    [Header("UI")]
    [SerializeField] public bool isChangingSettings;
    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject PosP1;
    [SerializeField] public GameObject PosP2;
    [SerializeField] public GameObject PosP3;
    [SerializeField] public GameObject PosP4;
    

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    /// <summary>
    /// Jump rebinding
    /// </summary>
    public void StartJumpRebinding()
    {
        startJumpRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindJumpComplete())
            .Start();
    }

    private void RebindJumpComplete()
    {
        int bindingIndex = jumpAction.action.GetBindingIndexForControl(jumpAction.action.controls[0]);

        bindingJumpDisplayNameText.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startJumpRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Dash Rebinding
    /// </summary>

    public void StartDodgeRebinding()
    {
        startDodgeRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = dodgeAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindDodgeComplete())
            .Start();
    }

    private void RebindDodgeComplete()
    {
        int bindingIndex = dodgeAction.action.GetBindingIndexForControl(dodgeAction.action.controls[0]);

        bindingDodgeDisplayNameText.text = InputControlPath.ToHumanReadableString(
            dodgeAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startDodgeRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Shoot Rebinding
    /// </summary>
    public void StartShootRebinding()
    {
        startShootRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = shootAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindShootComplete())
            .Start();
    }

    private void RebindShootComplete()
    {
        int bindingIndex = shootAction.action.GetBindingIndexForControl(shootAction.action.controls[0]);

        bindingShootDisplayNameText.text = InputControlPath.ToHumanReadableString(
            shootAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startShootRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Move Rebinding
    /// </summary>
    public void StartMoveRebinding()
    {
        startMoveRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = moveAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindMoveComplete())
            .Start();
    }

    private void RebindMoveComplete()
    {
        int bindingIndex = shootAction.action.GetBindingIndexForControl(shootAction.action.controls[0]);

        bindingMoveDisplayNameText.text = InputControlPath.ToHumanReadableString(
            moveAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startMoveRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Aim Rebinding
    /// </summary>
    public void StartAimRebinding()
    {
        startAimRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = aimAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindAimComplete())
            .Start();
    }

    private void RebindAimComplete()
    {
        int bindingIndex = aimAction.action.GetBindingIndexForControl(aimAction.action.controls[0]);

        bindingAimDisplayNameText.text = InputControlPath.ToHumanReadableString(
            aimAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startAimRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

    /// <summary>
    /// Aim Override Rebinding
    /// </summary>
    public void StartAimOverrideRebinding()
    {
        startAimOverrideRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Menu");

        rebindingOperation = aimOverrideAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindAimOverrideComplete())
            .Start();
    }

    private void RebindAimOverrideComplete()
    {
        int bindingIndex = aimOverrideAction.action.GetBindingIndexForControl(aimOverrideAction.action.controls[0]);

        bindingAimOverrideDisplayNameText.text = InputControlPath.ToHumanReadableString(
            aimOverrideAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startAimOverrideRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        playerMovement.PlayerInput.SwitchCurrentActionMap("Player");
    }

}
