using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDisplay : MonoBehaviour
{
    private Player player;
    private Health health;

    [Header("Health")]
    [SerializeField] private Image healthImage;
    [SerializeField] private TMP_Text healthText;

    [Header("Level")]
    [SerializeField] private Image experienceImage;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text levelText;

    public void InitializeUI(Player player)
    {
        this.player = player;
        health = player.GetComponent<Health>();

        health.OnTakeDamage += PlayerStatsDisplay_OnTakeDamage;
        player.OnExperienceGained += Player_OnExperienceGained;

        UpdateHealthUI();
        UpdateLevelUI();
    }

    public void UnSubscribeEvent()
    {
        health.OnTakeDamage -= PlayerStatsDisplay_OnTakeDamage;
    }

    private void PlayerStatsDisplay_OnTakeDamage(GameObject sender, bool hasImpact)
    {
        UpdateHealthUI();
    }

    private void Player_OnExperienceGained()
    {
        UpdateHealthUI();
        UpdateLevelUI();
    }

    private void UpdateHealthUI()
    {
        float healthAmount = health.GetHealth();
        float maxHealth = health.GetMaxHealth();

        healthImage.fillAmount = healthAmount / maxHealth;
        healthText.text = healthAmount.ToString("F1");
    }

    private void UpdateLevelUI()
    {
        float currentXP = player.Experience;
        float XPToLevelUp = player.GetExperienceToLevelUp();
        float XPToPreviousLevel = player.GetExperienceToPrevious();
        int currentLevel = player.GetLevel();

        experienceImage.fillAmount = (currentXP - XPToPreviousLevel) / XPToLevelUp;
        experienceText.text = $"{currentXP} / {XPToLevelUp}";

        levelText.text = $"Level: {currentLevel}";
    }
}
