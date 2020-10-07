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
        return GridManager.instance.GetCircleRange(owner.location,length);
    }
}
