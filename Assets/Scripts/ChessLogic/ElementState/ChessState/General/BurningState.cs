using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BurningState", menuName = "ElementState/BurningState")]
public class BurningState : ElementStateBase
{
    public override void Enter()
    {
        base.Enter();
        //owner.render.material.SetColor("_Color", Color.red);
        //owner.render.material.SetFloat("_Blend", 0.5f);
    }
    public override void OnHitElement(Element element)
    {
        base.OnHitElement(element);
        switch (element)
        {
            case Element.Ice:
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
}
