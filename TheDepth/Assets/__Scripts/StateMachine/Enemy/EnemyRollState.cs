using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRollState : EnemyBaseState
{
    public EnemyRollState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //Vector3 direction = stateMachine.Player.transform.position - stateMachine.transform.position;
        Vector3 direction = UnityEngine.Random.insideUnitSphere;
        //Vector3 dir = Vector3.Cross(-direction, Vector3.up).normalized;
        Vector3 dir = direction.normalized;
        
        Roll(dir);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName("Roll") || !stateMachine.Animator.IsInTransition(0))
        {   
            if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
        }
    }

    public override void Exit()
    {
        stateMachine.Animator.applyRootMotion = false;
    }

    private void Roll(Vector3 dir)
    {
        stateMachine.EnemyAnimator.PlayTargetAnimation("Roll", true);
        Quaternion rollRotation = Quaternion.LookRotation(dir);
        stateMachine.transform.rotation = rollRotation;
    }
}
