using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public event Action OnRoll;
    public event Action OnAttack;

    public bool IsAttacking { get; private set; }
    public bool IsAiming { get; private set; }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.ZoomIn.performed += ZoomIn_performed;
        playerInputActions.Player.ZoomIn.canceled += ZoomIn_canceled;
        playerInputActions.Player.ZoomOut.performed += ZoomOut_performed;
        playerInputActions.Player.ZoomOut.canceled += ZoomOut_canceled;

        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Attack.canceled += Attack_canceled;
        playerInputActions.Player.Attack.started += Attack_started; ;

        playerInputActions.Player.Aim.performed += Aim_performed;
        playerInputActions.Player.Aim.canceled += Aim_canceled;

        playerInputActions.Player.Roll.started += Roll_started;

        playerInputActions.Enable();
    }

    private void ZoomOut_canceled(InputAction.CallbackContext callback)
    {
        CameraController.Instance.SetZoomValue(0f);
    }

    private void ZoomIn_canceled(InputAction.CallbackContext callback)
    {
        CameraController.Instance.SetZoomValue(0f);
    }

    private void ZoomOut_performed(InputAction.CallbackContext callback)
    {
        CameraController.Instance.SetZoomValue(1f);
    }

    private void ZoomIn_performed(InputAction.CallbackContext callback)
    {
        CameraController.Instance.SetZoomValue(-1f);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        AimCameraController.Instance.SetMove(inputVector);
        return inputVector;
    }

    public void SetLookRotation()
    {
        Vector2 inputVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        CameraController.Instance.SetVectorRotation(inputVector);
        AimCameraController.Instance.SetLook(inputVector);
    }

    public Vector3 GetLookRotation()
    {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }

    private void Attack_started(InputAction.CallbackContext obj)
    {
        OnAttack?.Invoke();
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        IsAttacking = true;
    }

    private void Attack_canceled(InputAction.CallbackContext obj)
    {
        IsAttacking = false;
    }

    private void Aim_performed(InputAction.CallbackContext obj)
    {
        IsAiming = true;
        AimCameraController.Instance.SetAim(true);
    }

    private void Aim_canceled(InputAction.CallbackContext obj)
    {
        IsAiming = false;
        AimCameraController.Instance.SetAim(false);
    }

    private void Roll_started(InputAction.CallbackContext obj)
    {
        OnRoll?.Invoke();
    }
}
