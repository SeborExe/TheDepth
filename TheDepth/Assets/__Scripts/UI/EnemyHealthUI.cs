using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    private Health health;

    [SerializeField] private Image healthImage;
    private float maxHealth;

    public void InitializeUI(Health health)
    {
        this.health = health;

        health.OnTakeDamage += Health_OnTakeDamage;
        health.OnDie += Health_OnDie;

        UpdateHealthUI();
        maxHealth = health.GetMaxHealthValue();
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= Health_OnTakeDamage;
        health.OnDie -= Health_OnDie;
    }

    private void Health_OnDie()
    {
        gameObject.SetActive(false);
    }

    private void Health_OnTakeDamage(GameObject obj, bool hasImpact)
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        float healthAmount = health.GetHealth();
        healthImage.fillAmount = healthAmount / maxHealth;
    }
}
