using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AliveFloorState", menuName = "ElementState/FloorState/AliveFloorState")]
public class GrassFloorState : ElementStateBase
{
    public Mesh grassFloor;
    public override void Enter()
    {
        base.Enter();
        owner.meshFilter.mesh = grassFloor;
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Ice:
                stateMachine.SwitchState(ElementState.Frozen);
                break;
            default:
                break;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}

