using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalState", menuName = "ElementState/NormalState")]
public class NormalState : ElementStateBase
{
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Water:
                stateMachine.SwitchState(ElementState.Wet);
                break;
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Burning);
                break;
            default:
                break;
        }
    }
}
