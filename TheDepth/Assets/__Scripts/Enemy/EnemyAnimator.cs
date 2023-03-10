using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorController
{
    private readonly int SPEED = Animator.StringToHash("Speed");
    private readonly int IS_INTERACTING = Animator.StringToHash("IsInteracting");

    public Animator Animator { get; private set; }

    private CharacterController characterController;

    private float animationDampTime = 0.1f;
    private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
    }

    public void UpdateEnemyMoveAnimation(float speed, float deltaTime)
    {
        Animator.SetFloat(SPEED, speed, animationDampTime, deltaTime);
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

        Vector3 deltaPosition = Animator.deltaPosition;
        deltaPosition.y = 0;
        characterController.transform.position += deltaPosition;
    }
}
