using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private const float animationSmoothCrossFade = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.SetCurrentState(this);
        stateMachine.EnemyAnimator.CrossFadeAnimation(stateMachine.Animator, stateMachine.EnemyAnimator.LOCOMOTION_TREE, animationSmoothCrossFade);
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInChaseRange())
        {
            stateMachine.ChangePlayerDetection(null);
            stateMachine.SwitchState(new EnemySuspiciousState(stateMachine));
            return;
        }

        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        FacePlayer(deltaTime);

        stateMachine.EnemyAnimator.UpdateEnemyMoveAnimation(1f, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.NavMeshAgent.ResetPath();
        stateMachine.NavMeshAgent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.destination = stateMachine.Player.transform.position;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
    }

    private bool IsInAttackRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
