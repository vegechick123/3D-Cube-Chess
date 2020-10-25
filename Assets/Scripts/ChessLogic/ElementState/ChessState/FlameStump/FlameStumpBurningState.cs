using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameStumpBurningState", menuName = "ElementState/Character/FlameStump/FlameStumpBurningState")]
public class FlameStumpBurningState : ElementStateBase
{
    [NonSerialized]
    protected int radius = 2;
    public int coreTemperture = 3;
    private CHeatSource heatSource;
    public override void Enter()
    {
        base.Enter();
        //Vector2Int[] Range = GridManager.instance.GetCircleRange(owner.location,radius);
        //foreach(Vector2Int position in Range)
        //{
        //    GridManager.instance.GetFloor(position).ElementReaction(Element.Fire);
        //}
        //GChess[] chesses = GridManager.instance.GetChessesInRange(Range);
        //foreach (GChess chess in chesses)
        //{
        //    if(chess!=owner)
        //    {
        //        chess.ElementReaction(Element.Fire);
        //    }
        //}
        heatSource=owner.gameObject.AddComponent<CHeatSource>();
        heatSource.coreTempature = coreTemperture;
        owner.render.GetComponent<AudioSource>().Play();
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
        owner.render.GetComponent<AudioSource>().Stop(); ;
        base.Exit();
        Destroy(heatSource);

    }
}
