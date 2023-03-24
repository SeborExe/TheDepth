using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>, ISaveable
{
    [field: Header("Stats")]
    [field: SerializeField] public float Experience { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);
    }

    public void GainExperience(float XP)
    {
        Experience += XP;
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
