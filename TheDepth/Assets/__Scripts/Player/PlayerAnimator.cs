using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private CharacterController characterController;

    private readonly int FORWARD_SPEED = Animator.StringToHash("forwardSpeed");
    private readonly int LOCOMOTION_TREE = Animator.StringToHash("Locomotion Tree");
    private readonly int IS_INTERACTING = Animator.StringToHash("IsInteracting");

    public bool IsInteracting { get; private set; }
    public bool IsImmune;

    [SerializeField] private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        IsInteracting = Animator.GetBool(IS_INTERACTING);
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

    
    public void PlayTargetAnimation(string targetAnimation, bool IsInteracting)
    {
        Animator.applyRootMotion = IsInteracting;
        Animator.SetBool(IS_INTERACTING, IsInteracting);
        Animator.CrossFadeInFixedTime(targetAnimation, animationMoveSmooth);
    }


    private void OnAnimatorMove()
    {
        if (!Animator.GetBool(IS_INTERACTING)) return;

        float delta = Time.deltaTime;
        Vector3 deltaPosition = Animator.deltaPosition;
        deltaPosition.y = 0;
        characterController.transform.position += deltaPosition;
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
