using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    private float idleTimer = 0f;
    private const float IDLE_DURATION = 1f;

    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        idleTimer = 0f;
    }

    public override void HandleInput(){}
    public override void Update()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer >= IDLE_DURATION)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }

    }

    public override void PhysicsUpdate(){}
}