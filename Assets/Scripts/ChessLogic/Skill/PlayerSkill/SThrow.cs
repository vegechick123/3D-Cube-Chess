using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throw", menuName = "Skills/Throw")]
public class SThrow : PlayerSkill
{
    public int distance = 1;
    public int length = 1;
    public Element element = Element.None;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GChess chess,GFloor floor)
    {
        Vector2Int direction = chess.location - owner.location;

        TakeEffect(() =>
        {
            chess.ThrowTo(floor.location);
            chess.ElementReaction(element);
        },
        owner.location, chess.location);
    }
    public override bool ConditionCheck(int index, object[] parameters)
    {
        if (!base.ConditionCheck(index, parameters))
            return false;
        else
        {
            if (index == 0)
            {
                return (parameters[0] as GChess) != owner;
            }
            else if (index == 1)
            {
                Vector2Int loc = (parameters[1] as GFloor).location;
                if (GridManager.instance.GetChess(loc))
                    return false;
            }
            return true;
        }

    }

}
