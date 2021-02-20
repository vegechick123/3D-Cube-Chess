using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorState : State
{
    async public virtual UniTask OnHitElementAsync(Element element) { }
    async public virtual UniTask OnTurnEndAsync() { }
}
class NoneCover : FloorState
{
    async public override UniTask OnHitElementAsync(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                break;
            case Element.Ice:
                break;
            case Element.Water:
                owner.SetState(new WaterCover(this));
            case Element.Thunder:
                break;
            case Element.Oil:
                break;
            case Element.None:
                break;
        }
    }
}
class WaterCover : FloorState
{
    public override UniTask OnHitElementAsync(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                break;
            case Element.Ice:
                break;
            case Element.Water:
                break;
            case Element.Thunder:
                break;
            case Element.Oil:
                break;
            case Element.None:
                break;
        }
    }
}
class OilCover : FloorState
{
    public override UniTask OnHitElementAsync(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                break;
            case Element.Ice:
                break;
            case Element.Water:
                break;
            case Element.Thunder:
                break;
            case Element.Oil:
                break;
            case Element.None:
                break;
        }
    }
}
class FireCover : FloorState
{
    public override UniTask OnHitElementAsync(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                break;
            case Element.Ice:
                break;
            case Element.Water:
                break;
            case Element.Thunder:
                break;
            case Element.Oil:
                break;
            case Element.None:
                break;
        }
    }
}