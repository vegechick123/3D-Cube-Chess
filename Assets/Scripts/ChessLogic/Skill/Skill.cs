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
    public string title;
    public string info;
    [HideInInspector]
    public UnityEvent eBegin;
    [HideInInspector]
    public UnityEvent eEnd;
    public string GetInfo()
    {
        return info;
    }
    public virtual void Init(GChess owner)
    {
        this.owner = owner;
        skillVFX = Instantiate(skillVFX);
        skillVFX.Init(this);
    }    
    
    public string GetTitle()
    {
       return title;
    }
    
    protected Vector2Int[] GetRangeWithLength(int length)
    {
        return GridManager.instance.GetCircleRange(owner.location,length);
    }
    protected async virtual UniTask Shoot(Vector2Int location)
    {
        skillVFX.SetTarget(location);
        skillVFX.CreateShootParticle();
        await skillVFX.CreateProjectileParticle();
        skillVFX.CreateHitParticle();
    }
}

