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
