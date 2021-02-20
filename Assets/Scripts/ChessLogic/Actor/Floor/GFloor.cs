using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Ludiq;

public class GFloor : GActor
{
    public bool transitable = true;

    [NonSerialized]
    public bool combustible = false;//可燃的
    [NonSerialized]
    public bool readyToBurst = false;//用来BFS
    protected FloorStateMachine floorStateMachine;
    public FloorStateEnum defaultState;
    private void Awake()
    {
        floorStateMachine = new FloorStateMachine(this);
        floorStateMachine.SetState(defaultState);
    }
    public virtual void OnChessEnter(GChess chess)
    {
        floorStateMachine.currentState.OnChessEnter(chess);
    }
    public override void ElementReaction(Element element)
    {
        base.ElementReaction(element);
        floorStateMachine.currentState.OnHitElement(element);
    }
    public virtual void OnPlayerTurnEnd()
    {
        floorStateMachine.currentState.OnPlayerTurnEnd();
    }

}
