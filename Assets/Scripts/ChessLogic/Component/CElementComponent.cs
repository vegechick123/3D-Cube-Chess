using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;
using Ludiq;
using Bolt;
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
    protected override void Awake()
    {
        base.Awake();        
    }
    public virtual void OnHitElement(Element element)
    {
        CustomEvent.Trigger(gameObject, "OnHitElement",element);
    }
}
