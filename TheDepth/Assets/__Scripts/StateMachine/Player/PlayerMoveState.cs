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
        stateMachine.PlayerAnimator.PlayLocomotionTree(stateMachine.Animator);

        stateMachine.InputHandler.OnRoll += InputHandler_OnRoll;
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.PlayerAnimator.IsInteracting)
        {
            CheckForAttack();
            CheckForAiming();
        }

        CheckFormMove(deltaTime);
        stateMachine.InputHandler.SetLookRotation();
    }

    public override void Exit()
    {
        stateMachine.InputHandler.OnRoll -= InputHandler_OnRoll;
    }

    private void InputHandler_OnRoll()
    {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCoolDown) { return; }

        stateMachine.SetDodgeTime(Time.time);
        Roll();
    }

    private void CheckForAttack()
    {
        if (stateMachine.InputHandler.IsAttacking && !stateMachine.CurrentWeapon.isDistanceWeapon)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
    }

    private void CheckForAiming()
    {
        if (stateMachine.InputHandler.IsAiming && stateMachine.CurrentWeapon.isDistanceWeapon)
        {
            stateMachine.SwitchState(new PlayerAimingState(stateMachine));
            return;
        }
    }
    
    private void Roll()
    {
        Vector3 movement = CalculateMovement();
        if (stateMachine.InputHandler.GetMovementVectorNormalized() != Vector2.zero)
        {
            stateMachine.PlayerAnimator.PlayTargetAnimation("Roll", true);
            Quaternion rollRotation = Quaternion.LookRotation(movement);
            stateMachine.transform.rotation = rollRotation;
        }
        else
        {
            //Backstep?
        }
    }
}
