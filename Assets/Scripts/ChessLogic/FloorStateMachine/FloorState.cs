using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementSurface;
public enum FloorStateEnum
{
    NoneCover,
    WaterCover,
    OilCover,
    FireCover,
    Empty
}
public class FloorStateMachine
{
    public GFloor floor;
    public FloorState currentState;
    public FloorStateEnum currentStateEnum;
    public FloorStateMachine(GFloor owner)
    {
        floor = owner;
    }
    void SetState(FloorState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
        GChess chessOnFloor = GridManager.instance.GetChess(floor.location);
        if (chessOnFloor)
            currentState.OnChessEnter(chessOnFloor);
    }
    public void SetState(FloorStateEnum state)
    {
        currentStateEnum = state;
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
            case FloorStateEnum.Empty:
                SetState(new Empty(this));
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
        MarchingQuad.instance.SetTypeAndUpdateTexture(floor.location, MarchingQuad.ElementTotype(stateMachine.currentStateEnum));
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
    public virtual void Ignite()
    {

    }
    public virtual bool IsBurning()
    {
        return false;
    }
    public virtual bool IsWet()
    {
        return false;
    }
    public virtual void BurningUp()
    {
        stateMachine.SetState(FloorStateEnum.Empty);
    }
}

class Empty : FloorState
{
    public Empty(FloorStateMachine owner) : base(owner)
    {

    }
    public override void OnChessEnter(GChess chess)
    {
        chess.TryFall();
    }
    public override void Enter()
    {
        base.Enter();
        floor.transitable = false;
        floor.render.enabled = false;
    }
    public override void Exit()
    {
        floor.transitable = true;
        floor.render.enabled = true;
    }

}
class NoneCover : FloorState
{
    public NoneCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void OnHitElement(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                stateMachine.SetState(FloorStateEnum.FireCover);
                break;
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
    public override void Ignite()
    {
        stateMachine.SetState(FloorStateEnum.FireCover);
    }
}
class WaterCover : FloorState
{

    public WaterCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void Enter()
    {
        base.Enter();

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
    public override bool IsWet()
    {
        return true;
    }
}
class OilCover : FloorState
{
    public OilCover(FloorStateMachine owner) : base(owner)
    {

    }
    public override void Enter()
    {
        base.Enter();
        stateMachine.floor.explosive = true;
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
        stateMachine.floor.explosive = false;
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
        base.Enter();
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
                stateMachine.SetState(FloorStateEnum.NoneCover);
                break;
            case Element.Oil:
                Boom(0);
                break;
        }
    }
    public override bool IsBurning()
    {
        return true;
    }
    public override void Ignite()
    {
        stateMachine.SetState(FloorStateEnum.Empty);
    }
}
