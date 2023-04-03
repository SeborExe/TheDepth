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
        stateMachine.PlayerAnimator.PlayAimingTree(stateMachine.Animator);

        stateMachine.Animator.SetLayerWeight(1, 1);

        stateMachine.PlayerAnimator.SetOverrideAnimation(stateMachine.Animator, stateMachine.AimingOverrideController);
        MainGameCanvas.Instance.AimingUI.ChangeCrosshairVisibility(true);

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

        stateMachine.Animator.SetLayerWeight(1, 0);

        stateMachine.PlayerAnimator.SetOverrideAnimation(stateMachine.Animator, stateMachine.BaseOverrideController);
        MainGameCanvas.Instance.AimingUI.ChangeCrosshairVisibility(false);
    }

    private void InputHandler_OnAttack()
    {
        if (!stateMachine.PlayerAnimator.IsInteracting)
        {
            stateMachine.PlayerAnimator.PlayTargetAnimation("Bow_Shoot", true);
        }
    }
}
