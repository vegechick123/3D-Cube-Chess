using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FrozenFloorState", menuName = "ElementState/FloorState/FrozenFloorState")]
public class FrozenFloorState: ElementStateBase
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        return;
        switch (element)
        {
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Normal);
                break;
            default:
                break;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override int ProcessDamage(Element element, int damage)
    {
        if (element == Element.Fire)
            return 0;
        else
            return damage;
    }
}

