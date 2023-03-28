using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int startingLevel = 1;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private Progression progression;

    public int CurrentLevel { get; private set; } = 1;

    private void Start()
    {
        CurrentLevel = CalculateLevel();
    }

    public float GetStat(Stat stat)
    {
        return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
    }

    public float GetStat(Stat stat, int level)
    {
        return progression.GetStat(stat, characterClass, level) + GetAdditiveModifier(stat);
    }

    private float GetBaseStat(Stat stat)
    {
        return progression.GetStat(stat, characterClass, GetLevel());
    }

    public int GetLevel()
    {
        return CurrentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        CurrentLevel = level;
    }

    public int CalculateLevel()
    {
        if (TryGetComponent(out Player player))
        {
            float currentXP = player.Experience;
            int penultimateLevel = progression.GetLevel(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
        else
        {
            return startingLevel;
        }
    }

    private float GetAdditiveModifier(Stat stat)
    {
        float total = 0;
        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float modifier in provider.GetAdditiveModifier(stat))
            {
                total += modifier;
            }
        }

        return total;
    }

    private float GetPercentageModifier(Stat stat)
    {
        float total = 0;
        foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
        {
            foreach (float modifier in provider.GetPercentageModifier(stat))
            {
                total += modifier;
            }
        }

        return total;
    }
}
