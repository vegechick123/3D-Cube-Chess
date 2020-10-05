using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
public enum Element
{
    Fire,
    Water,
    Ice
}
public enum ElementState
{
    Burning,
    Normal,
    Wet,
    Frozen
}
public class CElementComponent : Component
{

    // Start is called before the first frame update

    public ElementStateBase BurningState;
    public ElementStateBase NormalState;
    public ElementStateBase WetState;
    public ElementStateBase FrozenState;
    public ElementState state = ElementState.Normal;
    protected ElementStateBase curStateObject;
    protected override void Awake()
    {
        base.Awake();
        curStateObject = GetNewStateObject(state);
        curStateObject.Init(actor, this);
        curStateObject.Enter();
    }
    public virtual void OnHitElement(Element element)
    {
        curStateObject.OnHitElement(element);
    }
    public virtual void SwitchState(ElementState newState)
    {
        Debug.Log("ElementState:"+newState);
        state = newState;
        curStateObject.Exit();
        curStateObject = GetNewStateObject(state);
        curStateObject.Init(actor, this);
        curStateObject.Enter();
    }
    protected ElementStateBase GetNewStateObject(ElementState newState)
    {
        switch (newState)
        {
            case ElementState.Burning:
                return Instantiate<ElementStateBase>(BurningState);                
            case ElementState.Normal:
                return Instantiate<ElementStateBase>(NormalState);
            case ElementState.Wet:
                return Instantiate<ElementStateBase>(WetState);
            case ElementState.Frozen:
                return Instantiate<ElementStateBase>(FrozenState);
            default:
                return null;
        }
    }
}
