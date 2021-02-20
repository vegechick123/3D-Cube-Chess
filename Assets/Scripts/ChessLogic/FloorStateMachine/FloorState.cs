using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FloorStateEnum
{
    NoneCover,
    WaterCover,
    OilCover,
    FireCover,
}
public class FloorStateMachine
{
    public GFloor floor;
    public FloorState currentState;

    public FloorStateMachine(GFloor owner)
    {
        floor = owner;
    }
    public void SetState(FloorState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public void SetState(FloorStateEnum state)
    {
        switch (state)
        {
            case FloorStateEnum.NoneCover:
                SetState(new NoneCover(this));
                break;
            case FloorStateEnum.WaterCover:
                SetState(new WaterCover(this));
                break;
            case FloorStateEnum.OilCover:
                SetState(new OilCover(this));
                break;
            case FloorStateEnum.FireCover:
                SetState(new FireCover(this));
                break;
        }
    }
}
public class FloorState
{
    protected FloorStateMachine stateMachine;
    public GFloor floor { get { return stateMachine.floor; } }
    public FloorState(FloorStateMachine owner)
    {
        this.stateMachine = owner;
    }
    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }
    public virtual void OnChessEnter(GChess chess)
    {
    }
    public virtual void OnPlayerTurnEnd()
    {

    }
    public virtual void OnHitElement(Element element) { }
    async public virtual UniTask OnTurnEndAsync() { }
}
class NoneCover : FloorState
{
    public NoneCover(FloorStateMachine owner) : base(owner)
    {

    }
    async public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Water:
                stateMachine.SetState(FloorStateEnum.WaterCover);
                break;
            case Element.Oil:
                stateMachine.SetState(FloorStateEnum.OilCover);
                break;
            case Element.None:
                break;
        }
    }
}
class WaterCover : FloorState
{
    public WaterCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void Enter()
    {
        stateMachine.floor.render.material.color = Color.blue;
    }
    public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                stateMachine.SetState(FloorStateEnum.NoneCover);
                break;
            case Element.Oil:
                stateMachine.SetState(FloorStateEnum.OilCover);
                break;
            case Element.None:
                break;
        }
    }
}
class OilCover : FloorState
{
    public OilCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void Enter()
    {
        stateMachine.floor.render.material.color = Color.black;
        stateMachine.floor.combustible = true;
    }
    public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                stateMachine.SetState(FloorStateEnum.FireCover);
                (stateMachine.currentState as FireCover).Boom(0);
                break;
            case Element.Water:
                stateMachine.SetState(FloorStateEnum.WaterCover);
                break;
        }
    }
    public override void Exit()
    {
        stateMachine.floor.render.material.color = Color.black;
        stateMachine.floor.combustible = false;
    }
}
class FireCover : FloorState
{
    public void Boom(int Damage)
    {
        GridManager.instance.GetChess(floor.location)?.Damage(1);
        stateMachine.floor.CreateFloatTextOnHead("爆炸", new Color(0.5f, 0, 0));
    }
    public override void Enter()
    {
        stateMachine.floor.render.material.color = Color.red;
    }
    public FireCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void OnPlayerTurnEnd()
    {
        GridManager.instance.GetChess(floor.location)?.Damage(1);
        stateMachine.floor.CreateFloatTextOnHead("燃烧", new Color(0.5f, 0, 0));
    }
    public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Ice:
            case Element.Water:
                stateMachine.SetState(new NoneCover(stateMachine));
                break;
            case Element.Oil:
                Boom(0);
                break;

        }
    }
}
