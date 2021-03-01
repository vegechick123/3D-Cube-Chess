using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Skill/AISkill/CloseAttack")]
public class SCloseAttack : AIPostSkill
{
    public int damage;
    public Element element; 
    protected Vector2Int direction; 
    public override Vector2Int[] GetTargetRange()
    {
        return GetRangeWithLength(1);
    }
    public override async UniTask<bool> Decide(GChess target)
    {
        if (target == null)
            return false;
        direction = target.location-owner.location;
        return true;
    }
    async public override UniTask ProcessAsync()
    {
        await ElementSystem.ApplyElementAtAsync(owner.location+direction,element,damage);
        GChess chess = GridManager.instance.GetChess(owner.location + direction);;
        Debug.Log(chess ? chess.ToString(): "Empty");
    }
    
    public override Vector2Int[] GetAffectRange()
    {
        return new Vector2Int[] { direction + owner.location };
    }

}
