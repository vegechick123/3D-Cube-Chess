﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
/// <summary>
/// 这是用附加在GAIChess上的决策组件
/// 它根据Chess的移动范围以及技能的释放范围决定两件事，1.在这个回合移动到哪里，2.以哪个Chess作为技能目标
/// 接着移动到相应位置，并把目标Chess作为参数传递给AISkill，让其做出具体决定
/// </summary>
public class CAICompoment : Component
{
    protected GAIChess AIChess { get { return actor as GAIChess; } }
    public GChess target;//目标Chess
    protected Vector2Int desination;//目标移动位置
    protected FloorHUD floorHUD;//用以显示AI技能的影响范围
    protected override void Awake()
    {
        base.Awake();
        AIManager.instance.AIs.Add(this);
    }
    /// <summary>
    /// AI根据移动范围以及技能释放范围决定
    /// </summary>
    public void Visit()
    {
        target = null;
        desination = AIChess.location;
        AIChess.navComponent.GenNavInfo();
        Vector2Int[] moveRange = AIChess.navComponent.GetMoveRangeWithoutOccupy();
        System.Random r = new System.Random();
        moveRange = moveRange.OrderBy(x => r.Next()).ToArray();
        var originLocation = AIChess.location;
        foreach (Vector2Int location in moveRange)
        {
            AIChess.location = location;//暂时更改location以供skill.GetRange来判断
            GChess[] targets = GridManager.instance.GetChesses(GameManager.instance.playerTeam);
            foreach (GChess curTarget in targets)
                if (AIChess.skill.GetRange().InRange(curTarget.location))
                {
                    if(curTarget.elementComponent.state==ElementState.Frozen)
                    {
                        continue;
                    }
                    target = curTarget;
                    desination = location;
                    break;
                }
        }
        AIChess.location = originLocation;
    }
    /// <summary>
    /// 实施移动，并调用AISkill.Decide
    /// </summary>
    async public UniTask PerformMove()
    {
        if (AIChess.location == desination)
        {
            return;
        }
        else
        {
            if (target != null)
            {
                await AIChess.MoveToAsync(desination);
                AIChess.skill.Decide(target);
            }
        }
    }
    /// <summary>
    /// 移动完成的回调函数，通知AIManager进行下一步操作
    /// </summary>
    /// <summary>
    /// 显示技能范围
    /// </summary>
    public void PrepareSkill()
    {
        if (target != null)
        {
            (actor as GChess).FaceToward((target.location - actor.location).Normalized());
            floorHUD = new FloorHUD(GetSkill().GetAffectRange, new Color(1, 0, 0, 0.8f));
            AIChess.skill.PreCast();
        }
        else
            Debug.Log("Target Miss");
    }
    /// <summary>
    /// 释放技能
    /// </summary>
    async public UniTask PerformSkill()
    {
        if(floorHUD!=null)
        {
            floorHUD.Release();
            floorHUD = null;
        }
        if (target != null)
        {
            await AIChess.skill.Perform();
        }
        
    }
    protected AISkill GetSkill()
    {
        return (actor as GAIChess).skill;
    }
    public void CancelSkill()
    {
        if (floorHUD!=null)
        {
            floorHUD.Release();
            floorHUD = null;
        }
        target = null;
    }
    private void OnDestroy()
    {
        Debug.Log(gameObject + "AIDestory");
        if(AIManager.instance)
            AIManager.instance.AIs.Remove(this);
        CancelSkill();
    }
}
