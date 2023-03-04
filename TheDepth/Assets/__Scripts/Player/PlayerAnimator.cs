using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private readonly int FORWARD_SPEED = Animator.StringToHash("forwardSpeed");

    [SerializeField] private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdatePlayerMoveAnimation(float speed, float deltaTime)
    {
        animator.SetFloat(FORWARD_SPEED, speed, animationMoveSmooth, deltaTime);
    }
}
