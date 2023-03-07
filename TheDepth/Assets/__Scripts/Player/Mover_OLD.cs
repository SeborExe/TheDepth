using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private PlayerAnimator playerAnimator;
    private InputHandler inputHandler;
    private PlayerController playerController;
    CharacterController characterController;
    ForceReciver forceReciver;

    [field: SerializeField] public bool IsKeyboardMovementEnabled { get; private set; }

    public float speed { get; private set; }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        inputHandler = GetComponent<InputHandler>();
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        forceReciver = GetComponent<ForceReciver>();
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

        MovePlayer(movement * speed, deltaTime);

        if (movement.magnitude == 0f)
        {
            playerAnimator.UpdatePlayerMoveAnimation(0f, deltaTime);
            return;
        }

        playerAnimator.UpdatePlayerMoveAnimation(speed, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    protected void MovePlayer(Vector3 motion, float deltaTime)
    {
        characterController.Move((motion + forceReciver.Movement) * deltaTime);
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
