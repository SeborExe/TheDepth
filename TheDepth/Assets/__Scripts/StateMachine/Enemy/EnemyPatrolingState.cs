using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolingState : EnemyBaseState
{
    private Vector3 nextPosition;
    private float waypointTolerance = 1f;
    private int currentWaypointIndex = 0;
    private float timeInWayPoint;

    public EnemyPatrolingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        nextPosition = GetCurrentWaypoint();
        stateMachine.NavMeshAgent.updatePosition = true;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if (PlayerDetected() || stateMachine.Player != null)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        if (stateMachine.PatrolPath != null)
        {
            if (AtWayPoint())
            {
                timeInWayPoint = stateMachine.TimeInPatrolCheckPoint;
                CycleWaypoint();
            }

            nextPosition = GetCurrentWaypoint();
        }

        if (timeInWayPoint < 0)
        {
            MoveToDestination(nextPosition, stateMachine.PercentSpeedWhenBackToPosition, deltaTime);
            FaceToDestination(nextPosition, deltaTime);

            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(stateMachine.PercentSpeedInPatrolingState, deltaTime);
        }
        else
        {
            timeInWayPoint -= deltaTime;
            stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(0f, deltaTime);
        }
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.updatePosition = false;
    }

    private bool AtWayPoint()
    {
        float distanceToWaypoint = Vector3.Distance(stateMachine.transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = stateMachine.PatrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return stateMachine.PatrolPath.GetWayPoint(currentWaypointIndex);
    }
}
