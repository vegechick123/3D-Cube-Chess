using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurningState", menuName = "ElementState/BurningState")]
public class BurningState : ElementStateBase
{
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Water:
            case Element.Ice:
                stateMachine.SwitchState(ElementState.Normal);
                break;
            default:
                break;
        }
    }
}
