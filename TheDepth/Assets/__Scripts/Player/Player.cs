using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>, ISaveable
{
    public event Action OnExperienceGained;

    [field: Header("Stats")]
    [field: SerializeField] public float Experience { get; private set; } = 0;

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
        if (newLevel > baseStats.CurrentLevel)
        {
            baseStats.SetCurrentLevel(newLevel);
            health.HealthOnLevelUp();
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

    public float GetExperienceToPreciousLevel()
    {
        if ((baseStats.GetLevel() - 1) == 0) { return 0; }

        return baseStats.GetStat(Stat.ExperienceToLevelUp, baseStats.GetLevel() - 1);
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
