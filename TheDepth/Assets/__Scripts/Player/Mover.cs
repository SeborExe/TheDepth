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
    private PlayerAnimator playerAnimator;
    private Transform mainCamera;
    private InputHandler inputHandler;

    [SerializeField] private bool isKeyboardMovementEnabled;

    public float speed { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        mainCamera = Camera.main.transform;
        inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        if (isKeyboardMovementEnabled)
        {
            Move();
            inputHandler.SetLookRotation();
            navMeshAgent.ResetPath();
        }
        else
        {
            GetPlayerLocomotionParameters();
        }
    }

    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.destination = destination;
    }

    private void GetPlayerLocomotionParameters()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        speed = localVelocity.z;
    }

    private void Move()
    {
        Vector2 inputVector = inputHandler.GetMovementVectorNormalized();
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
