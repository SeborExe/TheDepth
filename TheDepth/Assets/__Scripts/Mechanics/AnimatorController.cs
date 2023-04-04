using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorController : MonoBehaviour
{
    public readonly int LOCOMOTION_TREE = Animator.StringToHash("Locomotion Tree");
    public readonly int AIMING_TREE = Animator.StringToHash("Aiming Tree");

    public bool IsImmune { get; private set; }

    public void PlayLocomotionTree(Animator animator, float dampTime = 0.2f)
    {
        animator.CrossFadeInFixedTime(LOCOMOTION_TREE, dampTime);
    }

    public void PlayAimingTree(Animator animator, float dampTime = 0.2f)
    {
        animator.CrossFadeInFixedTime(AIMING_TREE, dampTime);
    }

    public void CrossFadeAnimation(Animator animator, int animationName, float transitionDuration)
    {
        animator.CrossFadeInFixedTime(animationName, transitionDuration);
    }

    public void CrossFadeAnimation(Animator animator, string animationName, float transitionDuration)
    {
        animator.CrossFadeInFixedTime(animationName, transitionDuration);
    }

    public void SetOverrideAnimation(Animator animator, AnimatorOverrideController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }

    public void EnableImmune()
    {
        IsImmune = true;
    }

    public void DisableImmune()
    {
        IsImmune = false;
    }
}
