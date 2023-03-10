using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private bool PlayerDetected()
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
                    stateMachine.ChangePlayerDetection(colliders[i].gameObject);
                    return true;
                }
            }
        }

        return false;
    }
}
