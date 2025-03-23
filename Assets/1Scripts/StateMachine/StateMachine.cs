using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState{
    void Enter();
    void Exit();
    void HandleInput();
    void Update();
    void PhysicsUpdate();
}

public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}