using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private readonly int FORWARD_SPEED = Animator.StringToHash("forwardSpeed");
    private readonly int LOCOMOTION_TREE = Animator.StringToHash("Locomotion Tree");

    [SerializeField] private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void UpdatePlayerMoveAnimation(float speed, float deltaTime)
    {
        Animator.SetFloat(FORWARD_SPEED, speed, animationMoveSmooth, deltaTime);
    }

    public void PlayLocomotionTree(float dampTime = 0.2f)
    {
        Animator.CrossFadeInFixedTime(LOCOMOTION_TREE, dampTime);
    }

    public void CrossFadeAnimation(string animationName, float transitionDuration)
    {
        Animator.CrossFadeInFixedTime(animationName, transitionDuration);
    }

    public void SetOverrideAnimation(AnimatorOverrideController animatorController)
    {
        Animator.runtimeAnimatorController = animatorController;
    }
}
