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
        owner.render.GetComponent<Animator>().speed = 0;
        GChess chess = (owner as GChess);
        chess.unableAct = true;
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
        chess.unableAct = false;
        owner.render.GetComponent<Animator>().speed = 1;
        base.Exit();
    }
}
