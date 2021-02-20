using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState;
    public void SetState(State newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
public class State
{
    protected StateMachine  owner;
    public State(StateMachine stateMachine)
    {
        owner = stateMachine;
    }
    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }
}
