using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerAnimator playerAnimator;
    private InputHandler inputHandler;
    private PlayerController playerController;

    [field: SerializeField] public bool IsKeyboardMovementEnabled { get; private set; }

    public float speed { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        inputHandler = GetComponent<InputHandler>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!IsKeyboardMovementEnabled)
        {
            Move(Time.deltaTime);
            inputHandler.SetLookRotation();
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

    private void Move(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        speed = movement.magnitude * playerController.PlayerMoveSpeed;

        CheckIfCanMove(speed, movement);
        //transform.position += movement * speed * deltaTime;

        float minimumInputValueToMove = 0.05f;
        if (movement.magnitude < minimumInputValueToMove)
        {
            playerAnimator.UpdatePlayerMoveAnimation(0f, deltaTime);
            return;
        }

        playerAnimator.UpdatePlayerMoveAnimation(speed, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    private void CheckIfCanMove(float moveSpeed, Vector3 moveDir)
    {
        float playerRadius = navMeshAgent.radius;
        float playerHeight = navMeshAgent.height;
        float moveDistance = moveSpeed * Time.deltaTime;
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
    }

    public Vector3 CalculateMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * inputHandler.GetMovementVectorNormalized().y + right * inputHandler.GetMovementVectorNormalized().x;
    }

    public void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement),
            deltaTime * 8f);
    }
}
