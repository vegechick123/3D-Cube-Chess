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
    Water,
    Thunder,
    Oil,
    None
}
public class CElementComponent : Component
{

    // Start is called before the first frame update    

    public bool conductive = false;
    public virtual void OnHitElement(Element element)
    {
        switch (element)
        {
            default:
                CustomEvent.Trigger(gameObject, "OnHitElement", element);
                break;
        }
    }
}
