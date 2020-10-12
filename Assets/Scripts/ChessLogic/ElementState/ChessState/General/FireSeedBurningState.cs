using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSeedBurningState", menuName = "ElementState/FireSeedBurningState")]
public class FireSeedBurningState : ElementStateBase
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
            default:
                break;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
