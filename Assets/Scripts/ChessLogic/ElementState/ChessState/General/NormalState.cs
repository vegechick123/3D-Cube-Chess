using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalState", menuName = "ElementState/NormalState")]
public class NormalState : ElementStateBase
{
    public bool frozenHitIce;
    public bool burningHitFire=true;
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
                if(frozenHitIce)
                {
                    stateMachine.SwitchState(ElementState.Frozen);
                }
                else
                 stateMachine.SwitchState(ElementState.Cold);
                break;
            case Element.Fire:
                if(burningHitFire)
                    stateMachine.SwitchState(ElementState.Burning);
                break;
            default:
                break;
        }
    }
}
