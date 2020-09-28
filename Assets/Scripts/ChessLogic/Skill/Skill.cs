using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill: ScriptableObject
{
    [HideInInspector]
    public GChess owner;
    public abstract Vector2Int[] GetRange();
    protected Vector2Int[] GetRangeWithLength(int length)
    {
        Queue<Vector2Int> res = new Queue<Vector2Int>();
        Vector2Int[] dir = new Vector2Int[4];
        dir[0] = new Vector2Int(1, 1);
        dir[1] = new Vector2Int(1, -1);
        dir[2] = new Vector2Int(-1, 1);
        dir[3] = new Vector2Int(-1, -1);
        for (int x = 0; x <= length; x++)
            for (int y = 0; x + y <= length; y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (x == 0 && y == 0 && i >= 1)
                    {
                        break;
                    }
                    if (x == 0 && i >= 2)
                    {
                        break;
                    }
                    if (y == 0 && (i == 1 || i == 3))
                    {
                        continue;
                    }

                    Vector2Int loc = new Vector2Int(x, y) * dir[i] + owner.location;
                    if (GridManager.instance.InRange(loc))
                        res.Enqueue(loc);
                }
            }
        return res.ToArray();
    }
}
