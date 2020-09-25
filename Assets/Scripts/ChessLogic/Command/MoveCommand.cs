using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveCommand : RangeCommand
{
    public MoveCommand(Vector2Int[] _range, GActor obj, Action<GFloor> action) : base(_range,obj, action)
    {
        
    }
    protected override bool SetCondition<T1>(T1 pa)
    {
        
        if (!base.SetCondition(pa))
            return false;
        GFloor target = pa as GFloor;
        //判断目标地面上是否已有棋子
        if (GridManager.instance.GetGChess(target.location))
            return  false;
        return true;
    }
}
