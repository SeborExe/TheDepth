using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Aiming...");
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        CheckFormMove(deltaTime, 5f);

        stateMachine.InputHandler.SetLookRotation();

        if (!stateMachine.InputHandler.IsAiming)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
