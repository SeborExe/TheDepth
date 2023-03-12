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
        if (stateMachine.PatrolPath != null)
        {
            destination = stateMachine.PatrolPath.GetWayPoint(0);
        }
        else
        {
            destination = stateMachine.startPosition;
        }

        timeInSuspiciousState = stateMachine.TimeInSuspiciousState;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (PlayerDetected() || stateMachine.Player != null)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        if (timeInSuspiciousState > 0)
        {
            timeInSuspiciousState -= deltaTime;
            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(0f, deltaTime);
        }
        else
        {
            if (stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new EnemyPatrolingState(stateMachine));
                return;
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

            }

            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(stateMachine.PercentSpeedWhenBackToPosition, deltaTime);
        }
    }

    public override void Exit()
    {

    }
}
