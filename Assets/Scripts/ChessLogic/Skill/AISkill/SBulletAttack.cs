using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseAttack", menuName = "Skills/AISkill/CloseAttack")]
public class SBulletAttack : AISkill
{
    protected Vector2Int direction; 
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(1);
    }
    public override void Decide(GChess target)
    {
        direction = target.location-owner.location;
    }
    public override void Perform()
    {
        GChess chess = GridManager.instance.GetChess(owner.location + direction);
        if(chess!=null)
        chess.PushToward(direction,1);
    }
    public override Vector2Int[] GetAffectRange()
    {
        return new Vector2Int[] { direction + owner.location };
    }
}
