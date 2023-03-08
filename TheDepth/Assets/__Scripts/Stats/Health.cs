using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private PlayerStateMachine playerStateMachine;

    private float health;

    private void Awake()
    {
        playerStateMachine= GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Dealdamage(float damage)
    {
        if (health <= 0) { return; }
        if (playerStateMachine.PlayerAnimator.IsImmune) { return; }

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            //Play dead anim.
        }
    }
}
