using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Health.InstantiateRagdoll(stateMachine.Ragdoll, stateMachine.CurrentWeapon);
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}
