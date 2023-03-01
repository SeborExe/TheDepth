using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Mover playerMover;

    //private const string IS_WALKING = "IsWalking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMover = GetComponentInParent<Mover>();
    }

    private void Update()
    {
        //animator.SetBool(IS_WALKING, playerMover.IsWalking());
    }
}
