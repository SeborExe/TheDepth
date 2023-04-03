using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>, ISaveable
{
    public event Action OnExperienceGained;

    [field: Header("Stats")]
    [field: SerializeField] public float Experience { get; private set; } = 0;
    [field: SerializeField] public GameObject FollowTarget { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject particlesOnLevelUp;

    private BaseStats baseStats;
    private Health health;

    protected override void Awake()
    {
        base.Awake();

        baseStats = GetComponent<BaseStats>();
        health = GetComponent<Health>();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        OnExperienceGained += Player_OnExperienceGained;
    }

    private void OnDisable()
    {
        OnExperienceGained -= Player_OnExperienceGained;
    }

    private void Player_OnExperienceGained()
    {
        int newLevel = baseStats.CalculateLevel();
        if (newLevel > baseStats.GetLevel())
        {
            baseStats.SetCurrentLevel(newLevel);
            health.HealthOnLevelUp();
            LevelUpEffect();
        }
    }

    public void GainExperience(float XP)
    {
        Experience += XP;
        OnExperienceGained?.Invoke();
    }

    public int GetLevel()
    {
        return baseStats.GetLevel();
    }

    public float GetExperienceToLevelUp()
    {
        return baseStats.GetStat(Stat.ExperienceToLevelUp);
    }

    public float GetExperienceToPrevious()
    {
        if ((baseStats.GetLevel() - 1) == 0) { return 0; }

        return baseStats.GetStat(Stat.ExperienceToLevelUp, baseStats.GetLevel() - 1);
    }

    private void LevelUpEffect()
    {
        Instantiate(particlesOnLevelUp, transform.position, Quaternion.identity);
    }

    public object CaptureState()
    {
        return Experience;
    }

    public void RestoreState(object state)
    {
        Experience = (float)state;
    }
}
