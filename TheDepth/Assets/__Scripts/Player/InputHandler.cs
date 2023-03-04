using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.ZoomIn.performed += ZoomIn_performed;
        playerInputActions.Player.ZoomIn.canceled += ZoomIn_canceled;
        playerInputActions.Player.ZoomOut.performed += ZoomOut_performed;
        playerInputActions.Player.ZoomOut.canceled += ZoomOut_canceled;
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
        return inputVector;
    }

    public void SetLookRotation()
    {
        Vector2 inputVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        CameraController.Instance.SetVectorRotation(inputVector);
    }
}
