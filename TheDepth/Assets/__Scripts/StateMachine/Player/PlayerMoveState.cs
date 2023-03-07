using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.PlayerAnimator.PlayLocomotionTree();
    }

    public override void Tick(float deltaTime)
    {
        CheckForAttack();
        CheckFormMove(deltaTime);
        stateMachine.InputHandler.SetLookRotation();
    }

    public override void Exit()
    {
        
    }

    private void CheckForAttack()
    {
        if (stateMachine.InputHandler.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
    }

    private void CheckFormMove(float deltaTime)
    {
        Vector3 movement = CalculateMovement();
        float speed = movement.magnitude * stateMachine.MovementSpeed;

        Move(movement * stateMachine.MovementSpeed, deltaTime);

        if (movement.magnitude == 0f)
        {
            stateMachine.PlayerAnimator.UpdatePlayerMoveAnimation(0f, deltaTime);
            return;
        }

        stateMachine.PlayerAnimator.UpdatePlayerMoveAnimation(speed, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }
}
