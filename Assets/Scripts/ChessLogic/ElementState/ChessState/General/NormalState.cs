using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalState", menuName = "ElementState/NormalState")]
public class NormalState : ElementStateBase
{
    public override void Enter()
    {
        base.Enter();
        owner.render.material.SetColor("_Color", Color.white);
        owner.render.material.SetFloat("_Blend", 0f);
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Ice:
                stateMachine.SwitchState(ElementState.Cold);
                break;
            case Element.Fire:
                stateMachine.SwitchState(ElementState.Burning);
                break;
            default:
                break;
        }
    }
}
