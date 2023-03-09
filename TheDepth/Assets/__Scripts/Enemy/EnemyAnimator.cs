using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorController
{
    private readonly int SPEED = Animator.StringToHash("Speed");

    public Animator Animator { get; private set; }

    private float animationDampTime = 0.1f;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void UpdateEnemyMoveAnimation(float speed, float deltaTime)
    {
        Animator.SetFloat(SPEED, speed, animationDampTime, deltaTime);
    }
}
