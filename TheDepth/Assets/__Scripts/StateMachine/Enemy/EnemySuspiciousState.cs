using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySuspiciousState : EnemyBaseState
{
    private Vector3 destination;
    private float timeInSuspiciousState;

    public EnemySuspiciousState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        destination = stateMachine.startPosition;
        timeInSuspiciousState = stateMachine.TimeInSuspiciousState;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (timeInSuspiciousState > 0)
        {
            timeInSuspiciousState -= deltaTime;
            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(0f, deltaTime);
        }
        else
        {
            MoveToDestination(destination, stateMachine.PercentSpeedWhenBackToPosition, deltaTime);
            FaceToDestination(destination, deltaTime);

            if (Vector3.Distance(stateMachine.transform.position, destination) <= 0.5f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }

            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(stateMachine.PercentSpeedWhenBackToPosition, deltaTime);
        }
    }

    public override void Exit()
    {

    }
}
