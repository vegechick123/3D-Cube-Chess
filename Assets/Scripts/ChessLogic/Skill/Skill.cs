using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill: ScriptableObject,IGetInfo
{
    [HideInInspector]
    public GChess owner;
    public SkillVFX skillVFX;
    public string title;
    public string info;
    public string GetInfo()
    {
        return info;
    }

    public abstract Vector2Int[] GetRange();

    public string GetTitle()
    {
       return title;
    }

    protected Vector2Int[] GetRangeWithLength(int length)
    {
        return GridManager.instance.GetCircleRange(owner.location,length);
    }
}
