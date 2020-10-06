using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WetState", menuName = "ElementState/WetState")]
public class ColdState : ElementStateBase
{
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Normal);
                break;
            case Element.Ice:
                stateMachine.SwitchState(ElementState.Frozen);
                break;
            default:
                break;
        }
    }
}
