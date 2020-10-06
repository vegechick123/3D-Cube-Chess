using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameStumpFrozenState", menuName = "ElementState/Character/FlameStump/FlameStumpFrozenState")]
public class FlameStumpFrozenState : ElementStateBase
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Burning);
                break;
            default:
                break;
        }
    }
}
