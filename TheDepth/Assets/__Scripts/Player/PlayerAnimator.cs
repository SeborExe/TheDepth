using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Mover playerMover;

    private const string FORWARD_SPEED = "forwardSpeed";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponentInParent<Mover>();
    }

    private void Update()
    {
        animator.SetFloat(FORWARD_SPEED, playerMover.speed);
    }

    public void UpdatePlayerMoveAnimation(float speed)
    {
        animator.SetFloat(FORWARD_SPEED, speed);
    }
}
