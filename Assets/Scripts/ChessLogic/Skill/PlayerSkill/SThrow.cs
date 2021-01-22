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
    public void Cast(GChess chess, GFloor floor)
    {
        Vector2Int direction = chess.location - owner.location;
        Vector2Int origin = chess.location;
        _ = chess.ThrowToAsync(floor.location);
        TakeEffect(() =>
        {
            chess.ElementReaction(element);
        },
        GridManager.instance.GetChessPosition3D(origin), GridManager.instance.GetChessPosition3D(floor.location) + new Vector3(0, 0.5f, 0));
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
