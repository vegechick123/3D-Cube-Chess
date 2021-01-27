using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CloseAttack", menuName = "Skills/AISkill/CloseAttack")]
public class SCloseAttack : AIPostSkill
{
    protected Vector2Int direction; 
    public override Vector2Int[] GetTargetRange()
    {
        return GetRangeWithLength(1);
    }
    public override async UniTask Decide(GChess target)
    {
        direction = target.location-owner.location;
    }
    async public override UniTask ProcessAsync()
    {
        
        GChess chess = GridManager.instance.GetChess(owner.location + direction);
        Debug.Log(chess ? "Empty":chess.ToString());
    }
    
    public override Vector2Int[] GetAffectRange()
    {
        return new Vector2Int[] { direction + owner.location };
    }

}
