using Cysharp.Threading.Tasks;
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
    protected UnityEvent eFinish=new UnityEvent();
    public string title;
    public string info;
    protected bool ready { get; set; }
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
        TakeEffect(task, GridManager.instance.GetChessPosition3D(origin), GridManager.instance.GetChessPosition3D(destination));
    }
    public virtual void TakeEffect(UnityAction task, Vector3 origin, Vector3 destination)
    {
        if (skillVFX != null)
        {
            skillVFX.eHit.AddListenerForOnce(eFinish.Invoke);
            skillVFX.eHit.AddListenerForOnce(task);

            skillVFX.Cast(origin, destination);
        }
        else
        {
            eFinish.Invoke();
            task();
        }
    }
    protected Vector2Int[] GetRangeWithLength(int length)
    {
        return GridManager.instance.GetCircleRange(owner.location,length);
    }
    public abstract  UniTask ProcessAsync();
    
}

