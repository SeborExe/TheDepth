using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    private PlayerStateMachine playerStateMachine;

    protected override void Awake()
    {
        base.Awake();

        playerStateMachine = GetComponent<PlayerStateMachine>();

        DontDestroyOnLoad(gameObject);
    }
}
