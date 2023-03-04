using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Mover playerMover;

    private readonly int FORWARD_SPEED = Animator.StringToHash("forwardSpeed");

    [SerializeField] private float animationMoveSmooth = 0.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponentInParent<Mover>();
    }

    private void Update()
    {
        animator.SetFloat(FORWARD_SPEED, playerMover.speed, animationMoveSmooth, Time.deltaTime);
    }

    public void UpdatePlayerMoveAnimation(float speed, float deltaTime)
    {
        animator.SetFloat(FORWARD_SPEED, speed, animationMoveSmooth, deltaTime);
    }
}
