using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CAgentComponent : Component
{
    bool bSelected { get { return PlayerControlManager.instance.selectedChess == actor; } }
    public UnityEvent eSelect = new UnityEvent();
    public UnityEvent eDeselect= new UnityEvent();
}
