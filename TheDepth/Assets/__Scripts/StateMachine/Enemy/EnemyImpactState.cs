using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int Impact = Animator.StringToHash("Damage_01");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;
    private GameObject sender;

    public EnemyImpactState(EnemyStateMachine stateMachine, GameObject sender) : base(stateMachine)
    {
        this.sender = sender;
    }

    public override void Enter()
    {
        stateMachine.EnemyAnimator.CrossFadeAnimation(stateMachine.Animator, Impact, CrossFadeDuration);
        if (stateMachine.Player == null)
        {
            stateMachine.ChangePlayerDetection(sender.GetComponent<Health>());
        }
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;
        if (duration <= 0f)
        {
            if (IsRollSuccessed(stateMachine.ChanceToRollAfterTakeDamage))
            {
                stateMachine.SwitchState(new EnemyRollState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
        }
    }

    public override void Exit()
    {

    }
}
