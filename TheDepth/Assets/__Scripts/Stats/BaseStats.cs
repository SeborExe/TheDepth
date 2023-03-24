using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private int startingLevel = 1;
    [SerializeField] private CharacterClass characterClass;
    [SerializeField] private Progression progression;

    public float GetHealth()
    {
        return progression.GetHealth(characterClass, startingLevel);
    }
}
