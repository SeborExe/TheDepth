using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Mover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private PlayerAnimator playerAnimator;
    private Transform mainCamera;

    [SerializeField] private bool isKeyboardMovementEnabled;

    public float speed { get; private set; }
    public float zoomValue { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        mainCamera = Camera.main.transform;
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.ZoomIn.performed += ZoomIn_performed;
        playerInputActions.Player.ZoomIn.canceled += ZoomIn_canceled;
        playerInputActions.Player.ZoomOut.performed += ZoomOut_performed;
        playerInputActions.Player.ZoomOut.canceled += ZoomOut_canceled;
        playerInputActions.Enable();
    }

    private void ZoomOut_canceled(InputAction.CallbackContext callback)
    {
        zoomValue = 0f;
    }

    private void ZoomIn_canceled(InputAction.CallbackContext callback)
    {
        zoomValue = 0f;
    }

    private void ZoomOut_performed(InputAction.CallbackContext callback)
    {
        zoomValue = 1f;
    }

    private void ZoomIn_performed(InputAction.CallbackContext callback)
    {
        zoomValue = -1f;
    }

    private void Update()
    {
        if (isKeyboardMovementEnabled)
        {
            Move();
            navMeshAgent.ResetPath();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCursor();
            }

            GetPlayerLocomotionParameters();
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector2 GetLookRotation()
    {
        Vector2 inputVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        return inputVector;
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit);
        if (hasHit)
        {
            navMeshAgent.destination = hit.point;
        }
    }

    private void GetPlayerLocomotionParameters()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        speed = localVelocity.z;
    }

    private void Move()
    {
        Vector2 inputVector = GetMovementVectorNormalized();
        Vector3 moveDir;
        moveDir = mainCamera.forward * inputVector.y;
        moveDir += mainCamera.right * inputVector.x;
        moveDir.Normalize();
        moveDir.y = 0;

        if (inputVector.magnitude < 0.5f && inputVector.magnitude >= 0.2f)
        {
            speed = 3f;
        }
        else if (inputVector.magnitude >= 0.5f)
        {
            speed = 6f;
        }
        else
        {
            speed = 0f;
        }

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = speed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can move only on X axis
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //Can move only on Z axis
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move 
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        //isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        playerAnimator.UpdatePlayerMoveAnimation(speed);
    }
}
