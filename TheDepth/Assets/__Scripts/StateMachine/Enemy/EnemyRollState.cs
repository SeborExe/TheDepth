using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRollState : EnemyBaseState
{
    private bool isRolling;

    public EnemyRollState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Vector3 direction = stateMachine.Player.transform.position - stateMachine.transform.position;
        Vector3 dir = Vector3.Cross(direction, Vector3.up).normalized;

        Roll(dir);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (isRolling)
        {
            if (GetNormalizedTime(stateMachine.Animator) >= 1f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
        }
    }

    public override void Exit()
    {

    }

    private void Roll(Vector3 dir)
    {
        stateMachine.EnemyAnimator.PlayTargetAnimation("Roll", true);
        Quaternion rollRotation = Quaternion.LookRotation(dir);
        stateMachine.transform.rotation = rollRotation;
        isRolling = true;
    }

}
