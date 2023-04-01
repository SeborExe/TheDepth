using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    private float slowdownWhenRoll = 10f;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharacterController.Move((motion + stateMachine.ForceReciver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void CheckFormMove(float deltaTime, float slowDown = 1f)
    {
        Vector3 movement = CalculateMovement();
        float speed = movement.magnitude * (stateMachine.MovementSpeed / slowDown);

        if (stateMachine.PlayerAnimator.IsInteracting)
        {
            movement /= slowdownWhenRoll;
        }

        Move(movement * (stateMachine.MovementSpeed / slowDown), deltaTime);

        if (movement.magnitude == 0f)
        {
            stateMachine.PlayerAnimator.UpdatePlayerMoveAnimation(0f, deltaTime);
            return;
        }

        stateMachine.PlayerAnimator.UpdatePlayerMoveAnimation(speed, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    protected Vector3 CalculateMovement()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputHandler.GetMovementVectorNormalized().y +
            right * stateMachine.InputHandler.GetMovementVectorNormalized().x;
    }

    protected void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationSpeed);
    }
}
