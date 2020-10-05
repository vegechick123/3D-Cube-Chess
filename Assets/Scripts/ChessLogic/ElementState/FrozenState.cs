using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FrozenState", menuName = "ElementState/FrozenState")]
public class FrozenState : ElementStateBase
{
    public Material snowMaterial;
    public override void Enter()
    {
        base.Enter();
        owner.render.material= snowMaterial;
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
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
        owner.render.material = owner.originMaterial;
        base.Exit();
    }
}
