using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float health;

    private void Start()
    {
        health = maxHealth;
    }

    public void Dealdamage(float damage)
    {
        if (health <= 0) { return; }

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            //Play dead anim.
        }
    }
}
