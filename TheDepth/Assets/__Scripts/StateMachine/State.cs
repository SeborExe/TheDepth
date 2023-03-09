using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) || nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }

        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0;
        }
    }

    protected void SetDamage(WeaponSO currentWeapon, WeaponDamage weaponDamage)
    {
        float damage = Random.Range(currentWeapon.minDamage, currentWeapon.maxDamage);
        weaponDamage.SetAttack(damage);
    }
}
