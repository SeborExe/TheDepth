using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBaseState
{
    private const float animationSmoothCrossFade = 1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.EnemyAnimator.CrossFadeAnimation(stateMachine.Animator, stateMachine.EnemyAnimator.LOCOMOTION_TREE, animationSmoothCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (PlayerDetected() || stateMachine.Player != null)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(0, deltaTime);
    }

    public override void Exit()
    {

    }
}
