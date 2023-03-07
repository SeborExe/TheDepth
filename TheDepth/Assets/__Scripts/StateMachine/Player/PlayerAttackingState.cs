using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private AttackSO attack;
    private float previousFrameTime;
    private bool alreadyAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.CurrentWeapon.attack[attackIndex];
    }

    public override void Enter()
    {
        SetDamage();
        stateMachine.PlayerAnimator.CrossFadeAnimation(attack.AnimationName, attack.TransitionDuration);
    }

    private void SetDamage()
    {
        float damage = UnityEngine.Random.Range(stateMachine.CurrentWeapon.minDamage, stateMachine.CurrentWeapon.maxDamage);
        stateMachine.WeaponDamage.SetAttack(damage);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        stateMachine.InputHandler.SetLookRotation();    

        float normalizedTime = GetNormalizedTime();

        if (normalizedTime > stateMachine.TimeWhilePlayerIsAbleToRotate && stateMachine.InputHandler.GetMovementVectorNormalized() != Vector2.zero)
        {
            Vector3 movement = CalculateMovement();
            FaceMovementDirection(movement, deltaTime);
        }

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputHandler.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }

        stateMachine.ForceReciver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if (attack.ComboStateIndex == -1) { return; }

        if (normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo currentInfo = stateMachine.PlayerAnimator.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = stateMachine.PlayerAnimator.Animator.GetNextAnimatorStateInfo(0);

        if (stateMachine.PlayerAnimator.Animator.IsInTransition(0) || nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }

        else if (!stateMachine.PlayerAnimator.Animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }
}
