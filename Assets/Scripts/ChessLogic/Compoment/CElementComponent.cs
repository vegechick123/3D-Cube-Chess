using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;

public enum Element
{
    Fire,
    Ice,
    None
}
public enum ElementState
{
    Burning,
    Normal,
    Cold,
    Frozen
}
public class CElementComponent : Component
{

    // Start is called before the first frame update

    public ElementStateBase BurningState;
    public ElementStateBase NormalState;
    public ElementStateBase ColdState;
    public ElementStateBase FrozenState;
    public ElementState state = ElementState.Normal;
    protected ElementStateBase curStateObject;
    public UnityEvent<ElementState> eStateEnter = new EventWrapper<ElementState>();
    protected override void Awake()
    {
        base.Awake();
    }
    public override void OnGameStart()
    {
        curStateObject = GetNewStateObject(state);
        curStateObject.Init(actor, this);
        curStateObject.disableParticle = true;
        curStateObject.Enter();
        curStateObject.disableParticle = false;
    }
    public virtual void OnHitElement(Element element)
    {
        curStateObject.OnHitElement(element);
    }
    public virtual int ProcessDamage(Element element, int damage)
    {
        return curStateObject.ProcessDamage(element,damage);
    }
    public virtual void SwitchState(ElementState newState)
    {
        Debug.Log("Enter to ElementState:"+newState);
        state = newState;
        curStateObject.Exit();
        curStateObject = GetNewStateObject(state);
        curStateObject.Init(actor, this);
        curStateObject.Enter();
        eStateEnter.Invoke(newState);
    }
    protected ElementStateBase GetNewStateObject(ElementState newState)
    {
        switch (newState)
        {
            case ElementState.Burning:
                return Instantiate<ElementStateBase>(BurningState);                
            case ElementState.Normal:
                return Instantiate<ElementStateBase>(NormalState);
            case ElementState.Cold:
                return Instantiate<ElementStateBase>(ColdState);
            case ElementState.Frozen:
                return Instantiate<ElementStateBase>(FrozenState);
            default:
                return null;
        }
    }
}
