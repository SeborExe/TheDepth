using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private AnimatorController animatorController;

    private float health;

    private void Awake()
    {
        animatorController = GetComponent<AnimatorController>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void Dealdamage(float damage)
    {
        if (health <= 0) { return; }
        //if (animatorController.IsImmune) { return; }

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            //Play dead anim.
        }
    }
}
