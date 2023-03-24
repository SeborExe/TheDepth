using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : SingletonMonobehaviour<PlayerStatsDisplay>
{
    private Health health;

    [Header("Health")]
    [SerializeField] private Image healthImage;
    [SerializeField] private TMP_Text healthText;
    private float maxHealth;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeUI(Health health)
    {
        this.health = health;

        health.OnTakeDamage += PlayerStatsDisplay_OnTakeDamage;
        UpdateHealthUI();

        maxHealth = health.MaxHealth;
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= PlayerStatsDisplay_OnTakeDamage;
    }

    private void PlayerStatsDisplay_OnTakeDamage(GameObject sender)
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        float healthAmount = health.GetHealth();

        healthImage.fillAmount = healthAmount / maxHealth;
        healthText.text = healthAmount.ToString("F1");
    }
}
