using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : ScriptableObject
{
    [NonSerialized]
    public GChess owner;
    [NonSerialized]
    public GChess caster;
    public virtual void InitByCaster(GChess caster)
    {
        this.caster = caster;
    }
    public virtual void InitByOwner(GChess owner)
    {
        this.owner = owner;
    }
    public virtual void OnDeath()
    {

    }
    public virtual void OnPassFloor(GFloor floor)
    {

    }
    protected void EndModifier()
    {
        owner.RemoveModifier(this);
    }
    public virtual void OnEnd()
    {
        
    }
    public virtual void OnPlayerTurn()
    {

    }
}
