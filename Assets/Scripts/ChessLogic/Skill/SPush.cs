using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Push", menuName = "Skills/Push")]
public class SPush : PlayerSkill
{
    public Vector2Int direction;
    public int distance=1;
    public int length=3;
    public override Vector2Int[] GetRange()
    {
        return GetRangeWithLength(length);
    }
    public void Cast(GChess chess)
    {
        chess.PushToward(direction, distance);
    }
}
