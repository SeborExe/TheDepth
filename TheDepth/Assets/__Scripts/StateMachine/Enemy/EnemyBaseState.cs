using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.CharacterController.Move((motion + stateMachine.ForceReciver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void MoveToDestination(Vector3 destination, float movementPercentSpeed, float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.destination = destination;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed * movementPercentSpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
    }

    protected void MoveToDestination(GameObject destinationObject, float deltaTime)
    {
        if (stateMachine.NavMeshAgent.isOnNavMesh)
        {
            stateMachine.NavMeshAgent.destination = destinationObject.transform.position;
            Move(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player.IsDead) { return; }

        Vector3 lookPosition = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

    protected void FacePlayer(float deltaTime)
    {
        if (stateMachine.Player.IsDead) { return; }

        Vector3 lookPosition = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(lookPosition),
            deltaTime * stateMachine.RotationSpeed);
    }

    protected void FaceToDestination(Vector3 destination, float deltaTime)
    {
        Vector3 lookPosition = destination - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(lookPosition),
            deltaTime * stateMachine.RotationSpeed);
    }

    protected void RotateToStartPosition(float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, stateMachine.startRotation,
            deltaTime * stateMachine.RotationSpeed);
    }

    protected bool PlayerDetected()
    {
        Collider[] colliders = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.PlayerDetectionRange,
            stateMachine.Detectionlayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform.TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                if (playerStateMachine.Health.IsDead) { return false; }

                Vector3 targetDirecton = playerStateMachine.transform.position - stateMachine.transform.position;
                float viewableAngle = Vector3.Angle(targetDirecton, stateMachine.transform.forward);
                if (viewableAngle > stateMachine.MinDetectionAngle && viewableAngle < stateMachine.MaxDetectionAngle)
                {
                    stateMachine.ChangePlayerDetection(colliders[i].GetComponent<Health>());
                    return true;
                }
            }
        }

        return false;
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float playerDistanceSqure = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        return playerDistanceSqure <= stateMachine.PlayerDetectionRange * stateMachine.PlayerDetectionRange;
    }

    protected bool CheckIfAnimationIsOver(string animationName)
    {
        return stateMachine.Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) || !stateMachine.Animator.IsInTransition(0);
    }

    protected bool IsRollSuccessed(int chance)
    {
        return chance > Random.Range(0, 100);
    }
}
