using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Skills/Push")]
public class SPush : PlayerSkill
{
    public int distance = 1;
    public int length = 1;
    public Element element = Element.None;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GChess chess)
    {
        Vector2Int direction = chess.location - owner.location;

        TakeEffect(() =>
        {
            chess.PushToward(direction, distance);
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
            else
                return true;
        }

    }

}
