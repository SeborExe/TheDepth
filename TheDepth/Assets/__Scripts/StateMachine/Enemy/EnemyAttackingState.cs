using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private AttackSO attack;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        int attackIndex = Random.Range(0, stateMachine.CurrentWeapon.attack.Length);
        attack = stateMachine.CurrentWeapon.attack[attackIndex];

        SetDamage(stateMachine.CurrentWeapon, stateMachine.WeaponDamage);
        stateMachine.EnemyAnimator.CrossFadeAnimation(stateMachine.Animator, attack.AnimationName, attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator) >= 1f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}
