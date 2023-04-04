using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvas : SingletonMonobehaviour<MainGameCanvas>
{
    [field: SerializeField] public PlayerStatsDisplay PlayerStatsDisplay { get; private set; }
    [field: SerializeField] public AimingUI AimingUI { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
