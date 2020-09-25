using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill: Skill
{
    public int length;
    public void Cast(GChess chess)
    {
        Debug.Log(chess);
    }
    public override Vector2Int[] GetRange()
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 1);
        dir[1] = new Vector2Int(1, -1);
        dir[2] = new Vector2Int(-1, 1);
        dir[3] = new Vector2Int(-1, -1);
        for (int x=0;x<=length;x++)
            for (int y = 0; x+y <= length;y++)
            {
                for(int i=0;i<4;i++)
                {
                    Vector2Int loc = new Vector2Int(x,y)*dir[i] + chess.location;
                    if (GridManager.instance.InRange(loc))
                        res.Enqueue(loc);
                }
            }
        return res.ToArray();
    }
}
