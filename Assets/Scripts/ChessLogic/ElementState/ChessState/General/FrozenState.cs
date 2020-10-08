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
        owner.render.material.SetColor("_Color", Color.white);
        owner.render.material.SetFloat("_Blend", 1f);
        owner.render.GetComponent<Animator>().speed = 0;
        GChess chess = (owner as GChess);
        chess.DisableAction();
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
        GChess chess = (owner as GChess);
        chess.ActiveAction();
        owner.render.GetComponent<Animator>().speed = 1;
        base.Exit();
    }
}
