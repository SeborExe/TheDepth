using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.PlayerAnimator.PlayLocomotionTree(stateMachine.Animator);
        stateMachine.PlayerAnimator.SetOverrideAnimation(stateMachine.Animator, stateMachine.AimingOverrideController);

        stateMachine.InputHandler.OnAttack += InputHandler_OnAttack;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        CheckFormMove(deltaTime, 5f);

        stateMachine.InputHandler.SetLookRotation();

        if (!stateMachine.InputHandler.IsAiming)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.InputHandler.OnAttack -= InputHandler_OnAttack;
        stateMachine.PlayerAnimator.SetOverrideAnimation(stateMachine.Animator, stateMachine.BaseOverrideController);
    }

    private void InputHandler_OnAttack()
    {
        if (!stateMachine.PlayerAnimator.IsInteracting)
        {
            stateMachine.PlayerAnimator.PlayTargetAnimation("Bow_Shoot", true);
        }
    }
}
