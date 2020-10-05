using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WetState", menuName = "ElementState/WetState")]
public class WetState : ElementStateBase
{
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Burning);
                break;
            case Element.Ice:
                stateMachine.SwitchState(ElementState.Frozen);
                break;
            default:
                break;
        }
    }
}
