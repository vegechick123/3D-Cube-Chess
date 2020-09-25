using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport", menuName = "Skills/Teleport")]
public class STeleport : Skill
{
    public int length = 3;
    public void Cast(GChess chess,GFloor floor)
    {
        chess.Teleport(floor.location);
    }

    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public override bool ConditionCheck(int index, object[] parameters)
    {
        if(index==1)
        {
            GFloor floor = parameters[1] as GFloor;
            return GridManager.instance.GetChess(floor.location)==null;
        }
        else
        {
            return true;
        }
    }
}
