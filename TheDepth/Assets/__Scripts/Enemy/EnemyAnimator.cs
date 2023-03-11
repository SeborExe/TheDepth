using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : AnimatorController
{
    private readonly int SPEED = Animator.StringToHash("Speed");
    private readonly int IS_INTERACTING = Animator.StringToHash("IsInteracting");

    public Animator Animator { get; private set; }
    public bool IsInteracting { get; private set; }

    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;

    private float animationDampTime = 0.1f;
    private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        IsInteracting = Animator.GetBool(IS_INTERACTING);
    }

    public void UpdateEnemyMoveAnimation(float speed, float deltaTime)
    {
        Animator.SetFloat(SPEED, speed, animationDampTime, deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation, bool IsInteracting)
    {
        Animator.applyRootMotion = IsInteracting;
        Animator.SetBool(IS_INTERACTING, IsInteracting);
        CrossFadeAnimation(Animator, targetAnimation, animationMoveSmooth);
    }

    private void OnAnimatorMove()
    {
        if (!Animator.GetBool(IS_INTERACTING)) return;

        float delta = Time.deltaTime;
        Vector3 deltaPosition = Animator.deltaPosition;
        deltaPosition.y = 0;
        characterController.transform.position += deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        navMeshAgent.velocity = velocity;
    }
}
