using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

//public class MoveCommand : RangeCommand
//{
//    public MoveCommand(Func<Vector2Int[]> GetRange, Func<GFloor,UniTask> action) : base(GetRange,1,action)
//    {
        
//    }
//    protected override bool SetCondition(GActor pa)
//    {
//        if (!base.SetCondition(pa))
//            return false;
//        GFloor target = pa as GFloor;
//        //判断目标地面上是否已有棋子
//        if (GridManager.instance.GetChess(target.location))
//            return  false;
//        return true;
//    }
//}
