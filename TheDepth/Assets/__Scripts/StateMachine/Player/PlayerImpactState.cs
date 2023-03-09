using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private readonly int Impact = Animator.StringToHash("Damage_01");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.PlayerAnimator.CrossFadeAnimation(stateMachine.Animator, Impact, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;
        if (duration <= 0f)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
