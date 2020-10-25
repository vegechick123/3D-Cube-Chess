using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    //protected abstract (Vector2Int, Vector2Int) GetVFXLocation();
    public string GetTitle()
    {
       return title;
    }
    public virtual void TakeEffect(UnityAction task,Vector2Int origin,Vector2Int destination)
    {
        if(skillVFX!=null)
        {
            skillVFX.eHit.AddListenerForOnce(task);

            skillVFX.Cast(origin, destination);
        }
        else
        {
            task();
        }
    }
    public virtual void TakeEffect(UnityAction task, Vector3 origin, Vector3 destination)
    {
        if (skillVFX != null)
        {
            skillVFX.eHit.AddListenerForOnce(task);

            skillVFX.Cast(origin, destination);
        }
        else
        {
            task();
        }
    }
    protected Vector2Int[] GetRangeWithLength(int length)
    {
        return GridManager.instance.GetCircleRange(owner.location,length);
    }
}
