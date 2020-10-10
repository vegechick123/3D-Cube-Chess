using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Skills/Push")]
public class SPush : PlayerSkill
{
    public int distance=1;
    public int length=1;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GChess chess)
    {
        Vector2Int direction = chess.location - owner.location;
        chess.PushToward(direction, distance);
    }
}
