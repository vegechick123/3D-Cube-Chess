using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Ludiq;

public class GFloor : GActor
{
    public bool transitable = true;
    public bool wooden = false;
    public bool inflammable=false;
    [NonSerialized]
    public bool conductive = false;//导电
    [NonSerialized]
    public bool explosive = false;//易爆
    [NonSerialized]
    public bool readyToBurst = false;//用来BFS

    public FloorStateMachine floorStateMachine;
    public FloorState state{ get { return floorStateMachine.currentState; } }
    public FloorStateEnum defaultState;

    public override void GAwake()
    {
        base.GAwake();
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
    public GChess GetChessAbove()
    {
        return GridManager.instance.GetChess(location);
    }
}
