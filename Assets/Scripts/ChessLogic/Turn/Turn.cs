using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public abstract class Turn : ScriptableObject
{
    protected TurnManager turnManager;
    public void Init(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }
    public virtual void OnEnterTurn() { }
    public virtual void OnExitTurn() { }
    public abstract UniTask TurnBehaviourAsync();
}
